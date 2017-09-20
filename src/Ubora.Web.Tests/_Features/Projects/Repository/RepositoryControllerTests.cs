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
using Ubora.Domain.Projects.Specifications;
using System.Linq.Expressions;
using Ubora.Web.Authorization;
using Ubora.Web.Tests.Helper;

namespace Ubora.Web.Tests._Features.Projects.Repository
{
    public class RepositoryControllerTests : ProjectControllerTestsBase
    {
        private readonly RepositoryController _controller;
        private readonly Mock<IUboraStorageProvider> _uboraStorageProviderMock;

        public RepositoryControllerTests()
        {
            _uboraStorageProviderMock = new Mock<IUboraStorageProvider>();
            _controller = new RepositoryController(_uboraStorageProviderMock.Object);
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
                .Set(x => x.Location, new BlobLocation("", ""));

            var projectFile2 = new ProjectFile()
                .Set(x => x.ProjectId, ProjectId)
                .Set(x => x.Location, new BlobLocation("", ""));

            var expectedProjectFiles = new List<ProjectFile>()
            {
                projectFile1,
                projectFile2
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

            var projectFileViewModels = new List<ProjectFileViewModel>();
            foreach (var file in expectedProjectFiles)
            {
                var expectedFile = new ProjectFileViewModel();
                AutoMapperMock
                    .Setup(m => m.Map<ProjectFileViewModel>(file))
                    .Returns(expectedFile);
                projectFileViewModels.Add(expectedFile);
            }

            var expectedModel = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = "Title",
                Files = projectFileViewModels,
                AddFileViewModel = new AddFileViewModel()
                {
                    ActionName = "AddFile",
                },
                IsProjectLeader = false
            };

            // Act
            var result = (ViewResult)_controller.Repository();

            // Assert
            result.ViewName.Should().Be(nameof(RepositoryController.Repository));
            result.Model.As<ProjectRepositoryViewModel>().ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public async Task AddFile_Executes_AddFileCommand_When_Valid_ModelState()
        {
            var fileMock = new Mock<IFormFile>();

            var model = new AddFileViewModel
            {
                ProjectFile = fileMock.Object
            };

            var fileName = "fileName";
            fileMock.Setup(f => f.FileName)
                .Returns($"C:\\Test\\Parent\\Parent\\{fileName}");

            var stream = Mock.Of<Stream>();
            fileMock.Setup(f => f.OpenReadStream())
                .Returns(stream);

            AddFileCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddFileCommand>()))
                .Callback<AddFileCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

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

            var model = new AddFileViewModel
            {
                ProjectFile = fileMock.Object
            };

            var fileName = "fileName";
            fileMock.Setup(f => f.FileName)
                .Returns(fileName);

            var stream = Mock.Of<Stream>();
            fileMock.Setup(f => f.OpenReadStream())
                .Returns(stream);

            var commandResult = new CommandResult("testError1", "testError2");
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
                .Returns(new CommandResult());

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
            var addFileViewModel = new AddFileViewModel
            {
                FileId = fileId,
                ProjectFile = fileMock.Object,
            };

            var fileName = "fileName";
            fileMock.Setup(f => f.FileName)
                .Returns($"C:\\Test\\Parent\\Parent\\{fileName}");

            var stream = Mock.Of<Stream>();
            fileMock.Setup(f => f.OpenReadStream())
                .Returns(stream);

            var projectFile = new ProjectFile();
            projectFile.Set(f => f.Id, fileId);
            QueryProcessorMock.Setup(q => q.FindById<ProjectFile>(fileId))
                .Returns(projectFile);

            UpdateFileCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<UpdateFileCommand>()))
                .Callback<UpdateFileCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

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
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.Id.Should().Be(fileId);
        }

        [Fact]
        public async Task UpdateFile_Returns_UpdateFile_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();

            var fileId = Guid.NewGuid();
            var addFileViewModel = new AddFileViewModel
            {
                FileId = fileId,
                ProjectFile = fileMock.Object,
            };

            var fileName = "fileName";
            fileMock.Setup(f => f.FileName)
                .Returns($"C:\\Test\\Parent\\Parent\\{fileName}");

            var projectFile = new ProjectFile();

            QueryProcessorMock.Setup(q => q.FindById<ProjectFile>(fileId))
                .Returns(projectFile);

            var commandResult = new CommandResult("testError1", "testError2");
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

            var model = new AddFileViewModel()
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

        private void CreateTestProject()
        {
            var expectedProject = new Project().Set(x => x.Title, "Title");

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(expectedProject);
        }
    }
}
