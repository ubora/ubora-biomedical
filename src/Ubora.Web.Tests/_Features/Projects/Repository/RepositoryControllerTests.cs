using System;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Repository;
using Ubora.Web._Features.Projects.Repository;
using Ubora.Web.Infrastructure.Storage;
using Xunit;
using System.Linq.Expressions;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Authorization;
using Ubora.Domain.Infrastructure.Queries;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Repository.Commands;
using Ubora.Domain.Projects.Repository.Events;
using Ubora.Web.Tests.Helper;

namespace Ubora.Web.Tests._Features.Projects.Repository
{
    public class RepositoryControllerTests : ProjectControllerTestsBase
    {
        private readonly RepositoryController _controller;
        private readonly Mock<IUboraStorageProvider> _uboraStorageProviderMock;
        private readonly Mock<IEventStreamQuery> _eventStreamQueryMock;

        public RepositoryControllerTests()
        {
            _uboraStorageProviderMock = new Mock<IUboraStorageProvider>();
            _eventStreamQueryMock = new Mock<IEventStreamQuery>();
            _controller = new RepositoryController(_uboraStorageProviderMock.Object, _eventStreamQueryMock.Object);
            SetUpForTest(_controller);
        }

        [Fact]
        public override void Actions_Have_Authorize_Attributes()
        {
            var methodPolicies = new List<AuthorizationTestHelper.RolesAndPoliciesAuthorization>
                {
                    new AuthorizationTestHelper.RolesAndPoliciesAuthorization
                    {
                        MethodName = nameof(RepositoryController.HideFile),
                        Policies = new []{ Policies.CanHideProjectFile}
                    }
                };

            AssertHasAuthorizeAttributes(typeof(RepositoryController), methodPolicies);
        }

        [Fact]
        public void Repository_Returns_View()
        {
            var projectFile1 = new ProjectFile()
                .Set(x => x.ProjectId, ProjectId)
                .Set(x => x.Location, new BlobLocation("", ""))
                .Set(x => x.FolderName, "folder1");

            var projectFile2 = new ProjectFile()
                .Set(x => x.ProjectId, ProjectId)
                .Set(x => x.Location, new BlobLocation("", ""))
                .Set(x => x.FolderName, "folder2");

            var projectFile3 = new ProjectFile()
                .Set(x => x.ProjectId, ProjectId)
                .Set(x => x.Location, new BlobLocation("", ""))
                .Set(x => x.FolderName, "folder2");

            var expectedProjectFiles = new List<ProjectFile>()
            {
                projectFile1,
                projectFile2,
                projectFile3
            };

            var project = new Project();
            project.Set(p => p.Id, ProjectId);
            project.Set(p => p.Title, "Title");
            var projectMembers = new List<ProjectMember> { new ProjectLeader(UserId) };
            project.Set(p => p.Members, projectMembers);

            var specification = new IsProjectFileSpec(ProjectId)
                    && !new IsHiddenFileSpec();
            QueryProcessorMock
                .Setup(x => x.Find(specification))
                .Returns(expectedProjectFiles);

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            var expectedFile = new ProjectFileViewModel();
            foreach (var file in expectedProjectFiles)
            {
                AutoMapperMock
                    .Setup(m => m.Map<ProjectFileViewModel>(file))
                    .Returns(expectedFile);
            }

            var projectFilesViewModel = expectedProjectFiles
                .GroupBy(f => f.FolderName, f => expectedFile);
            
            var expectedModel = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = "Title",
                AllFiles = projectFilesViewModel,
                AddFileViewModel = new AddFileViewModel(),
                IsProjectLeader = false
            };

            // Act
            var result = (ViewResult)_controller.Repository();

            // Assert
            result.ViewName.Should().Be(nameof(RepositoryController.Repository));
            result.Model.As<ProjectRepositoryViewModel>().ShouldBeEquivalentTo(expectedModel);
            result.Model.As<ProjectRepositoryViewModel>().AllFiles
                .First(f => f.Key == "folder1")
                .Count().Should().Be(1);

            result.Model.As<ProjectRepositoryViewModel>().AllFiles
                .First(f => f.Key == "folder2")
                .Count().Should().Be(2);
        }

        [Fact]
        public async Task AddFile_Executes_AddFileCommand_When_Valid_ModelState()
        {
            var fileMock = new Mock<IFormFile>();
            var comment = "comment";

            var model = new AddFileViewModel
            {
                ProjectFiles = new List<IFormFile> { fileMock.Object },
                Comment = comment
            };

            var fileName = "fileName";
            fileMock.Setup(f => f.FileName)
                .Returns($"C:\\Test\\Parent\\Parent\\{fileName}");

            var fileSize = 1234;
            fileMock.Setup(f => f.Length)
                .Returns(fileSize);

            var stream = Mock.Of<Stream>();
            fileMock.Setup(f => f.OpenReadStream())
                .Returns(stream);

            AddFileCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddFileCommand>()))
                .Callback<AddFileCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var expectedBlobLocation = BlobLocations.GetRepositoryFileBlobLocation(ProjectId, fileName);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName
                && b.BlobPath.Contains(fileName) && b.BlobPath.Contains(ProjectId.ToString());

            //Act
            var result = (RedirectToActionResult)await _controller.AddFile(model);

            //Assert
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.BlobLocation.BlobPath.Should()
                .StartWith($"{ProjectId}/repository/")
                // Guid in the middle.
                .And.EndWith(fileName);
            executedCommand.FileName.Should().Be(fileName);
            executedCommand.Comment.Should().Be(comment);
            executedCommand.FileSize.Should().Be(fileSize);

            result.ActionName.Should().Be(nameof(RepositoryController.Repository));

            _uboraStorageProviderMock.Verify(x => x.SavePrivate(It.Is(expectedBlobLocationFunc), stream));
        }

        [Fact]
        public async Task AddFile_Returns_Repository_View_With_ModelState_Errors_When_Form_Post_Is_Not_Valid()
        {
            var fileMock = new Mock<IFormFile>();

            var model = new AddFileViewModel();

            var errorMessage = "testError";
            _controller.ModelState.AddModelError("", errorMessage);
            fileMock.Setup(f => f.FileName)
                .Returns("C:\\Test\\Parent\\Parent\\image.png");
            QueryProcessorMock.Setup(p => p.Find(new IsProjectFileSpec(ProjectId) && !new IsHiddenFileSpec()))
                .Returns(new List<ProjectFile>());

            CreateTestProject();

            //Act
            var result = (ViewResult)await _controller.AddFile(model);

            //Assert
            result.ViewName.Should().Be(nameof(RepositoryController.Repository));

            AssertModelStateContainsError(result, errorMessage);

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);

            _uboraStorageProviderMock.Verify(x => x.SavePrivate(It.IsAny<BlobLocation>(), It.IsAny<Stream>()), Times.Never);
        }

        [Fact]
        public async Task AddFile_Returns_Repository_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();

            var comment = "comment";
            var model = new AddFileViewModel
            {
                ProjectFiles = new List<IFormFile> { fileMock.Object },
                Comment = comment
            };

            var fileName = "fileName";
            fileMock.Setup(f => f.FileName)
                .Returns(fileName);

            var stream = Mock.Of<Stream>();
            fileMock.Setup(f => f.OpenReadStream())
                .Returns(stream);

            var commandResult = CommandResult.Failed("testError1", "testError2");
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddFileCommand>()))
                .Returns(commandResult);
            QueryProcessorMock.Setup(p => p.Find(new IsProjectFileSpec(ProjectId) && !new IsHiddenFileSpec()))
                .Returns(new List<ProjectFile>());

            var expectedBlobLocation = BlobLocations.GetRepositoryFileBlobLocation(ProjectId, fileName);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName
                && b.BlobPath.Contains(fileName) && b.BlobPath.Contains(ProjectId.ToString());

            CreateTestProject();

            //Act
            var result = (ViewResult)await _controller.AddFile(model);

            //Assert
            result.ViewName.Should().Be(nameof(RepositoryController.Repository));

            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());

            _uboraStorageProviderMock.Verify(x => x.SavePrivate(It.Is(expectedBlobLocationFunc), stream), Times.Once);
        }

        [Fact]
        public void HideFile_Redirects_To_Repository_And_Hides_File()
        {
            var fileId = Guid.NewGuid();
            var executedCommand = new HideFileCommand();

            CommandProcessorMock.Setup(p => p.Execute(It.IsAny<HideFileCommand>()))
                .Callback<HideFileCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            //Act
            var result = (RedirectToActionResult)_controller.HideFile(fileId);

            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.Id.Should().Be(fileId);

            result.ActionName.Should().Be(nameof(Repository));
        }

        [Fact]
        public void DownloadFile_Returns_RedirectToUrl()
        {
            var projectFile = new ProjectFile();
            var blobLocation = new BlobLocation("container", "path");
            projectFile.Set(f => f.Location, blobLocation);

            var fileId = Guid.NewGuid();
            QueryProcessorMock.Setup(p => p.FindById<ProjectFile>(fileId))
                .Returns(projectFile);

            var expectedBlobSasUrl = "expectedBlobSasUrl";
            _uboraStorageProviderMock.Setup(p => p.GetReadUrl(blobLocation, It.IsAny<DateTime>()))
                .Returns(expectedBlobSasUrl);

            //Act
            var result = (RedirectResult)_controller.DownloadFile(fileId);

            //Assert
            result.Url.Should().Be(expectedBlobSasUrl);
        }

        [Fact]
        public void UpdateFile_Returns_View()
        {
            var fileId = Guid.NewGuid();
            var projectFile = new ProjectFile();
            QueryProcessorMock.Setup(q => q.FindById<ProjectFile>(fileId))
                .Returns(projectFile);

            var expectedViewModel = new UpdateFileViewModel();
            AutoMapperMock.Setup(m => m.Map<UpdateFileViewModel>(projectFile))
                .Returns(expectedViewModel);

            // Act
            var result = (ViewResult)_controller.UpdateFile(fileId);

            // Assert
            result.Model.Should().Be(expectedViewModel);
        }

        [Fact]
        public async Task UpdateFile_Updates_File_Location_And_Redirects_To_Repository()
        {
            var fileId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            var comment = "comment";
            var addFileViewModel = new UpdateFileViewModel
            {
                ProjectFile = fileMock.Object,
                FileId = fileId,
                Comment = comment
            };

            var fileName = "fileName";
            fileMock.Setup(f => f.FileName)
                .Returns($"C:\\Test\\Parent\\Parent\\{fileName}");

            var stream = Mock.Of<Stream>();
            fileMock.Setup(f => f.OpenReadStream())
                .Returns(stream);

            var fileSize = 1234;
            fileMock.Setup(f => f.Length)
                .Returns(fileSize);

            var projectFile = new ProjectFile();
            projectFile.Set(f => f.Id, fileId);

            QueryProcessorMock.Setup(q => q.FindById<ProjectFile>(fileId))
                .Returns(projectFile);

            UpdateFileCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<UpdateFileCommand>()))
                .Callback<UpdateFileCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var expectedBlobLocation = BlobLocations.GetRepositoryFileBlobLocation(ProjectId, fileName);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName
                && b.BlobPath.Contains(fileName) && b.BlobPath.Contains(ProjectId.ToString());

            // Act
            var result = (RedirectToActionResult)await _controller.UpdateFile(addFileViewModel);

            // Assert
            _uboraStorageProviderMock.Verify(p => p.SavePrivate(It.Is(expectedBlobLocationFunc), stream));

            result.ActionName.Should().Be(nameof(RepositoryController.Repository));

            executedCommand.BlobLocation.BlobPath.Should()
                .StartWith($"{ProjectId}/repository/")
                // Guid in the middle.
                .And.EndWith(fileName);
            executedCommand.FileSize.Should().Be(fileSize);
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.Id.Should().Be(fileId);
            executedCommand.Comment.Should().Be(comment);
        }

        [Fact]
        public async Task UpdateFile_Returns_UpdateFile_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();

            var fileId = Guid.NewGuid();
            var addFileViewModel = new UpdateFileViewModel
            {
                FileId = fileId,
                ProjectFile = fileMock.Object,
                Comment = "comment",
            };

            var fileName = "fileName";
            fileMock.Setup(f => f.FileName)
                .Returns($"C:\\Test\\Parent\\Parent\\{fileName}");

            var projectFile = new ProjectFile();
            var location = new BlobLocation("containerName", "blobPath");
            projectFile.Set(f => f.Location, location);

            QueryProcessorMock.Setup(q => q.FindById<ProjectFile>(fileId))
                .Returns(projectFile);

            var commandResult = CommandResult.Failed("testError1", "testError2");
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<UpdateFileCommand>()))
                .Returns(commandResult);

            var expectedViewModel = new UpdateFileViewModel();
            AutoMapperMock.Setup(m => m.Map<UpdateFileViewModel>(projectFile))
                .Returns(expectedViewModel);

            //Act
            var result = (ViewResult)await _controller.UpdateFile(addFileViewModel);

            //Assert
            result.ViewName.Should().Be(nameof(RepositoryController.UpdateFile));
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }

        [Fact]
        public async Task UpdateFile_Returns_UpdateFile_View_With_ModelState_Errors_When_Form_Post_Is_Not_Valid()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName)
                .Returns("C:\\Test\\Parent\\Parent\\image.png");

            var model = new UpdateFileViewModel()
            {
                FileId = Guid.NewGuid()
            };

            var projectFile = new ProjectFile();
            QueryProcessorMock.Setup(q => q.FindById<ProjectFile>(model.FileId))
                .Returns(projectFile);

            var expectedViewModel = new UpdateFileViewModel();
            AutoMapperMock.Setup(m => m.Map<UpdateFileViewModel>(projectFile))
                .Returns(expectedViewModel);

            var errorMessage = "testError";
            _controller.ModelState.AddModelError("", errorMessage);

            //Act
            var result = (ViewResult)await _controller.UpdateFile(model);

            //Assert
            result.ViewName.Should().Be(nameof(RepositoryController.UpdateFile));
            AssertModelStateContainsError(result, errorMessage);
        }

        [Fact]
        public void FileHistory_Returns_View_With_Expected_Model()
        {
            var fileId = Guid.NewGuid();
            var userInfo = new UserInfo(UserId, "user name");
            var fileAddedEvent = new FileAddedEvent(
                id: fileId,
                comment: "comment",
                fileName: "fileName",
                fileSize: 111,
                folderName: "folderName",
                initiatedBy: userInfo,
                location: new BlobLocation("containerName", "blobPath"),
                projectId: ProjectId,
                revisionNumber: 1
                );

            var fileUpdatedEvent = new FileUpdatedEvent(
                id: fileId,
                projectId: ProjectId,
                location: new BlobLocation("containerName2", "blobPath2"),
                comment: "comment2",
                fileSize: 222,
                initiatedBy: userInfo,
                revisionNumber: 2
                );

            var fileUpdatedEvent2 = new FileUpdatedEvent(
                id: fileId,
                projectId: ProjectId,
                location: new BlobLocation("containerName3", "blobPath3"),
                comment: "comment3",
                fileSize: 333,
                initiatedBy: userInfo,
                revisionNumber: 3
                );

            var fileAddedDate = DateTimeOffset.Now;
            var fileUpdatedDate = DateTimeOffset.Now.AddDays(1);
            var fileUpdated2Date = DateTimeOffset.Now.AddDays(2);
            var events = new List<IEvent>
            {
                new TestEvent{ Timestamp = fileAddedDate, Data = fileAddedEvent },
                new TestEvent{ Timestamp = fileUpdatedDate, Data = fileUpdatedEvent },
                new TestEvent{ Timestamp = fileUpdated2Date, Data = fileUpdatedEvent2 },
            };

            _eventStreamQueryMock.Setup(q => q.FindFileEvents(ProjectId, fileId))
                .Returns(events);

            var downLoadUrl = "downloadUrl";
            _uboraStorageProviderMock.Setup(p => p.GetReadUrl(It.IsAny<BlobLocation>(), It.IsAny<DateTime>()))
                .Returns(downLoadUrl);

            var projectFile = new ProjectFile();
            projectFile.Set(f => f.FileName, "fileName");
            QueryProcessorMock.Setup(q => q.FindById<ProjectFile>(fileId))
                .Returns(projectFile);

            var project = new Project();
            project.Set(p => p.Id, ProjectId);
            project.Set(p => p.Title, "Title");

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            var expectedFiles = new List<FileItemHistoryViewModel>
            {
                new FileItemHistoryViewModel
                {
                    Comment = "comment",
                    DownloadUrl = downLoadUrl,
                    FileAddedOn = fileAddedDate,
                    FileSize = 111,
                    RevisionNumber = 1
                },
                new FileItemHistoryViewModel
                {
                    Comment = "comment2",
                    DownloadUrl = downLoadUrl,
                    FileAddedOn = fileUpdatedDate,
                    FileSize = 222,
                    RevisionNumber = 2
                },
                new FileItemHistoryViewModel
                {
                    Comment = "comment3",
                    DownloadUrl = downLoadUrl,
                    FileAddedOn = fileUpdated2Date,
                    FileSize = 333,
                    RevisionNumber = 3
                }
            };

            var expectedViewModel = new FileHistoryViewModel
            {
                FileName = projectFile.FileName,
                ProjectName = project.Title,
                Files = expectedFiles.OrderByDescending(x => x.FileAddedOn)
            };

            // Act
            var result = (ViewResult) _controller.FileHistory(fileId);

            // Assert
            result.ViewName.Should().Be(nameof(RepositoryController.FileHistory));
            result.Model.As<FileHistoryViewModel>().ShouldBeEquivalentTo(expectedViewModel);
        }

        private void CreateTestProject()
        {
            var expectedProject = new Project().Set(x => x.Title, "Title");

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(expectedProject);
        }
    }
}
