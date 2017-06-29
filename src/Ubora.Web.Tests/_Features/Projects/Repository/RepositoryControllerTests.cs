using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Repository;
using Ubora.Web._Features.Projects.Repository;
using Ubora.Web.Tests.Helper;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Repository
{
    public class RepositoryControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICommandQueryProcessor> _commandQueryProcessorMock;
        private readonly Mock<IStorageProvider> _storageProviderMock;

        private readonly RepositoryController _controller;

        public RepositoryControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _commandQueryProcessorMock = new Mock<ICommandQueryProcessor>();
            _storageProviderMock = new Mock<IStorageProvider>();
            _controller = new RepositoryController(_commandQueryProcessorMock.Object, _mapperMock.Object, _storageProviderMock.Object);
            SetProjectAndUserContext(_controller);
        }

        [Fact]
        public void Repository_Returns_View()
        {
            var projectFile1 = new ProjectFile()
                .SetPropertyValue(nameof(ProjectFile.ProjectId), ProjectId)
                .SetPropertyValue(nameof(ProjectFile.Location), new BlobLocation("", ""));

            var projectFile2 = new ProjectFile()
                .SetPropertyValue(nameof(ProjectFile.ProjectId), ProjectId)
                .SetPropertyValue(nameof(ProjectFile.Location), new BlobLocation("", ""));

            var otherProjetFile = new ProjectFile()
                .SetPropertyValue(nameof(ProjectFile.Location), new BlobLocation("", ""));

            var allProjectFiles = new List<ProjectFile>()
            {
                projectFile1,
                projectFile2,
                otherProjetFile
            };

            _commandQueryProcessorMock.Setup(x => x.Find<ProjectFile>(null))
                .Returns(allProjectFiles);

            CreateTestProject();

            var expectedProjectFiles = new List<ProjectFile>()
            {
                projectFile1,
                projectFile2,
            };

            var projectFileViewModels = new List<ProjectFileViewModel>();
            foreach (var file in expectedProjectFiles)
            {
                var expectedFile = new ProjectFileViewModel
                {
                    FileLocation = "testFileLocation"
                };
                _mapperMock.Setup(m => m.Map<ProjectFileViewModel>(file))
                    .Returns(expectedFile);
                projectFileViewModels.Add(expectedFile);
            }

            // TODO(Kaspar Kallas): Test more thoroughly
            _storageProviderMock
                .Setup(x => x.GetBlobUrl(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("testFileLocation");

            var expectedModel = new ProjectRepositoryViewModel
            {
                ProjectId = ProjectId,
                ProjectName = "Title",
                Files = projectFileViewModels,
            };

            // Act
            var result = (ViewResult) _controller.Repository();

            // Assert
            result.ViewName.Should().Be(nameof(RepositoryController.Repository));
            result.Model.As<ProjectRepositoryViewModel>().ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public void AddFile_Executes_AddFileCommand_When_Valid_ModelState()
        {
            var fileMock = new Mock<IFormFile>();

            var model = new AddFileViewModel
            {
                ProjectFile = fileMock.Object
            };

            AddFileCommand executedCommand = null;

            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");
            _commandQueryProcessorMock.Setup(p => p.Execute(It.IsAny<AddFileCommand>()))
                .Callback<AddFileCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            //Act
            var result = (RedirectToActionResult)_controller.AddFile(model);

            //Assert
            executedCommand.ProjectId.Should().Be(ProjectId);
            result.ActionName.Should().Be(nameof(RepositoryController.Repository));
        }

        [Fact]
        public void AddFile_Returns_Repository_View_With_ModelState_Errors_When_Form_Post_Is_Not_Valid()
        {
            var fileMock = new Mock<IFormFile>();

            var model = new AddFileViewModel();

            _controller.ModelState.AddModelError("", "testError");
            fileMock.Setup(f => f.FileName).Returns("C:\\Test\\Parent\\Parent\\image.png");

            CreateTestProject();

            //Act
            var result = (ViewResult)_controller.AddFile(model);

            //Assert
            result.ViewName.Should().Be(nameof(RepositoryController.Repository));
            _commandQueryProcessorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public void AddFile_Returns_Repository_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var fileMock = new Mock<IFormFile>();

            var model = new AddFileViewModel
            {
                ProjectFile = fileMock.Object
            };

            fileMock.Setup(f => f.FileName)
                .Returns("C:\\Test\\Parent\\Parent\\image.png");

            var commandResult = new CommandResult("testError1", "testError2");
            _commandQueryProcessorMock.Setup(p => p.Execute(It.IsAny<AddFileCommand>()))
                .Returns(commandResult);

            CreateTestProject();

            //Act
            var result = (ViewResult)_controller.AddFile(model);

            //Assert
            result.ViewName.Should().Be(nameof(RepositoryController.Repository));
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }


        private void CreateTestProject()
        {
            var expectedProject = new Project();
            expectedProject.SetPropertyValue(nameof(Project.Title), "Title");
            _commandQueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(expectedProject);
        }
    }
}
