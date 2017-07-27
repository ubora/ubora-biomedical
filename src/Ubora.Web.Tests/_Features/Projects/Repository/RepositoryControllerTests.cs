using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Repository;
using Ubora.Web._Features.Projects.Repository;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web.Tests.Helper;
using Xunit;
using Ubora.Domain.Projects.Queries;

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
        public void Actions_Have_Authorize_Attributes()
        {
            AssertHasAttribute(typeof(RepositoryController), nameof(RepositoryController.AddFile),
                typeof(AuthorizeAttribute));
            AssertHasAttribute(typeof(RepositoryController), nameof(RepositoryController.DownloadFile),
                typeof(AuthorizeAttribute));
            AssertHasAttribute(typeof(RepositoryController), nameof(RepositoryController.HideFile),
                typeof(AuthorizeAttribute));
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

            var otherProjetFile = new ProjectFile()
                .Set(x => x.Location, new BlobLocation("", ""));

            var expectedProjectFiles = new List<ProjectFile>()
            {
                projectFile1,
                projectFile2,
            };

            QueryProcessorMock
                .Setup(x => x.ExecuteQuery(It.IsAny<GetAvailableProjectFilesQuery>()))
                .Returns(expectedProjectFiles);

            CreateTestProject();

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
                    ActionName = "AddFile"
                }
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

            AddFileCommand executedCommand = null;

            var fileName = "fileName";
            fileMock.Setup(f => f.FileName).Returns($"C:\\Test\\Parent\\Parent\\{fileName}");
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddFileCommand>()))
                .Callback<AddFileCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            //Act
            var result = (RedirectToActionResult)await _controller.AddFile(model);

            //Assert
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.BlobLocation.BlobPath.Should()
                .StartWith($"{ProjectId}/repository/")
                // Guid in the middle.
                .And.EndWith(fileName);
            result.ActionName.Should().Be(nameof(RepositoryController.Repository));

            _uboraStorageProviderMock.Verify(x => x.SavePrivateStreamToBlobAsync(It.IsAny<BlobLocation>(), It.IsAny<Stream>()), Times.Once);
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

            CreateTestProject();

            //Act
            var result = (ViewResult)await _controller.AddFile(model);

            //Assert
            result.ViewName.Should().Be(nameof(RepositoryController.Repository));

            AssertModelStateContainsError(result, errorMessage);

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);

            _uboraStorageProviderMock.Verify(x => x.SavePrivateStreamToBlobAsync(It.IsAny<BlobLocation>(), It.IsAny<Stream>()), Times.Never);
        }

        [Fact]
        public async Task AddFile_Returns_Repository_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();

            var model = new AddFileViewModel
            {
                ProjectFile = fileMock.Object
            };

            fileMock.Setup(f => f.FileName)
                .Returns("C:\\Test\\Parent\\Parent\\image.png");

            var commandResult = new CommandResult("testError1", "testError2");
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<AddFileCommand>()))
                .Returns(commandResult);

            CreateTestProject();

            //Act
            var result = (ViewResult)await _controller.AddFile(model);

            //Assert
            result.ViewName.Should().Be(nameof(RepositoryController.Repository));

            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());

            _uboraStorageProviderMock.Verify(x => x.SavePrivateStreamToBlobAsync(It.IsAny<BlobLocation>(), It.IsAny<Stream>()), Times.Once);
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
            var fileId = Guid.NewGuid();
            QueryProcessorMock.Setup(p => p.FindById<ProjectFile>(fileId)).Returns(new ProjectFile());

            var expectedBlobSasUrl = "expectedBlobSasUrl";
            _uboraStorageProviderMock.Setup(p => p.GetBlobSasUrl(It.IsAny<BlobLocation>(), It.IsAny<DateTime>()))
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

            QueryProcessorMock.Setup(q => q.FindById<ProjectFile>(fileId))
                .Returns(new ProjectFile());

            UpdateFileCommand executedCommand = null;
            CommandProcessorMock
                .Setup(p => p.Execute(It.IsAny<UpdateFileCommand>()))
                .Callback<UpdateFileCommand>(c => executedCommand = c)
                .Returns(new CommandResult());



            // Act
            var result = (RedirectToActionResult)await _controller.UpdateFile(addFileViewModel);

            // Assert
            _uboraStorageProviderMock.Verify(p => p.SavePrivateStreamToBlobAsync(It.IsAny<BlobLocation>(), It.IsAny<Stream>()), Times.Once);

            result.ActionName.Should().Be(nameof(RepositoryController.Repository));

            executedCommand.BlobLocation.BlobPath.Should()
                .StartWith($"{ProjectId}/repository/")
                // Guid in the middle.
                .And.EndWith(fileName);
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
