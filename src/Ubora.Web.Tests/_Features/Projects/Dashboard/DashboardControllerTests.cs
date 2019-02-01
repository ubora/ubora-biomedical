using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;
using Ubora.Web._Features.Projects.Dashboard;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Dashboard
{
    public class DashboardControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<IStorageProvider> _storageProviderMock;
        private readonly Mock<ImageStorageProvider> _imageResizerMock;
        private readonly DashboardController _dashboardController;

        public DashboardControllerTests()
        {
            _storageProviderMock = new Mock<IStorageProvider>();
            _imageResizerMock = new Mock<ImageStorageProvider>();

            _dashboardController = new DashboardController(
                _storageProviderMock.Object,
                _imageResizerMock.Object);

            SetUpForTest(_dashboardController);
        }

        [Fact]
        public async Task EditProjectImage_Saves_New_Image_And_Creates_New_Resized_Ones_And_Redirects_To_Dashboard()
        {
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<UpdateProjectImageCommand>()))
                .Returns(CommandResult.Success);

            var file = Mock.Of<IFormFile>();
            var model = new EditProjectImageViewModel { Image = file };
            var expectedBlobLocation = BlobLocations.GetProjectImageBlobLocation(ProjectId);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName && b.BlobPath == expectedBlobLocation.BlobPath;

            // Act
            var result = await _dashboardController.EditProjectImage(model);

            // Assert
            _imageResizerMock.Verify(v => v.SaveImageAsync(null, It.Is(expectedBlobLocationFunc), SizeOptions.AllDefaultSizes));

            var actionResult = (RedirectToActionResult)result;
            actionResult.ActionName.Should().Be(nameof(DashboardController.Dashboard));
        }

        [Fact]
        public async Task EditProjectImage_Redirects_To_View_If_Modelstate_Has_Error()
        {
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<UpdateProjectImageCommand>()))
                .Returns(CommandResult.Failed("Error"));

            var fileMock = Mock.Of<IFormFile>(x => x.FileName == "image.jpg");
            var model = new EditProjectImageViewModel { Image = fileMock };

            // Act
            var result = await _dashboardController.EditProjectImage(model);

            // Assert
            _dashboardController.ModelState.ErrorCount
                .Should().Be(1);
        }

        [Fact]
        public async Task RemoveProjectImage_Removes_Image()
        {
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<DeleteProjectImageCommand>()))
                .Returns(CommandResult.Success);

            var model = new RemoveProjectImageViewModel();
            var expectedBlobLocation = BlobLocations.GetProjectImageBlobLocation(ProjectId);
            Expression<Func<BlobLocation, bool>> expectedBlobLocationFunc = b => b.ContainerName == expectedBlobLocation.ContainerName && b.BlobPath == expectedBlobLocation.BlobPath;

            // Act
            var result = await _dashboardController.RemoveProjectImage(model);

            // Assert
            _imageResizerMock.Verify(x => x.DeleteImagesAsync(It.Is(expectedBlobLocationFunc)));

            var actionResult = (RedirectToActionResult)result;
            actionResult.ActionName.Should().Be(nameof(DashboardController.Dashboard));
        }

        [Fact]
        public async Task RemoveProjectImage_Redirects_To_View_If_ModelState_Has_Errors()
        {
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<DeleteProjectImageCommand>()))
                .Returns(CommandResult.Failed("Error"));

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(new Project());

            var model = new RemoveProjectImageViewModel();

            // Act
            var result = await _dashboardController.RemoveProjectImage(model);

            // Assert
            _dashboardController.ModelState.ErrorCount
                .Should().Be(1);
        }

        [Fact]
        public async Task EditProjectTitleAndDescription_Returns_EditProjectTitleAndDescription_View_With_Expected_Model()
        {
            var project = new Project();
            project.Set(x => x.Title, "title");
            project.Set(x => x.DescriptionV2, new QuillDelta("{description}"));

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            NodeServicesMock
                .Setup(ns => ns.InvokeAsync<string>("./Scripts/backend/SanitizeQuillDelta.js", project.DescriptionV2.Value))
                .ReturnsAsync("DescriptionQuillDelta");

            var expectedModel = new EditProjectTitleAndDescriptionViewModel()
            {
                DescriptionQuillDelta = "DescriptionQuillDelta",
                Title = project.Title
            };

            // Act
            var result = (ViewResult) await _dashboardController.EditProjectTitleAndDescription();

            // Assert
            result.ViewName.Should().Be(nameof(DashboardController.EditProjectTitleAndDescription));
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public async Task EditProjectTitleAndDescription_Returns_EditProjectTitleAndDescription_View_With_ModelState_Errors_When_Model_Is_Invalid()
        {
            var project = new Project();
            project.Set(x => x.Title, "title");
            project.Set(x => x.DescriptionV2, new QuillDelta("{description}"));

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            var errorMessage = "errorMessage";
            _dashboardController.ModelState.AddModelError("", errorMessage);

            var model = new EditProjectTitleAndDescriptionViewModel
            {
                Title = "newTitle",
                DescriptionQuillDelta = "{newDescription}"
            };

            // Act
            var result = (ViewResult)await _dashboardController.EditProjectTitleAndDescription(model);

            // Assert
            result.ViewName.Should().Be(nameof(DashboardController.EditProjectTitleAndDescription));
            AssertModelStateContainsError(result, errorMessage);

            CommandProcessorMock.Verify(x => x.Execute(It.IsAny<UpdateProjectTitleAndDescriptionCommand>()), Times.Never);
        }

        [Fact]
        public async Task EditProjectTitleAndDescription_Returns_EditProjectTitleAndDescription_View_With_ModelState_Errors_When_Command_Not_Executed_Successfully()
        {
            var project = new Project();
            project.Set(x => x.Title, "title");
            project.Set(x => x.DescriptionV2, new QuillDelta("{description}"));

            QueryProcessorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(project);

            var commandResult = CommandResult.Failed("testError1", "testError2");
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<UpdateProjectTitleAndDescriptionCommand>()))
                .Returns(commandResult);

            var model = new EditProjectTitleAndDescriptionViewModel
            {
                Title = "newTitle",
                DescriptionQuillDelta = "{newDescription}"
            };

            // Act
            var result = (ViewResult)await _dashboardController.EditProjectTitleAndDescription(model);

            // Assert
            result.ViewName.Should().Be(nameof(DashboardController.EditProjectTitleAndDescription));
            AssertModelStateContainsError(result, commandResult.ErrorMessages.ToArray());
        }

        [Fact]
        public async Task EditProjectTitleAndDescription_Redirects_To_Dashboard_When_Command_Executed_Successfully()
        {
            var executedCommand = new UpdateProjectTitleAndDescriptionCommand();
            CommandProcessorMock.Setup(p => p.Execute(It.IsAny<UpdateProjectTitleAndDescriptionCommand>()))
                .Callback<UpdateProjectTitleAndDescriptionCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var model = new EditProjectTitleAndDescriptionViewModel
            {
                Title = "title",
                DescriptionQuillDelta = "{description}"
            };

            // Act
            var result = (RedirectToActionResult)await _dashboardController.EditProjectTitleAndDescription(model);

            // Assert
            result.ActionName.Should().Be(nameof(DashboardController.Dashboard));

            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.Description.Should().Be(new QuillDelta(model.DescriptionQuillDelta));
            executedCommand.Title.Should().Be(model.Title);
        }
    }
}
