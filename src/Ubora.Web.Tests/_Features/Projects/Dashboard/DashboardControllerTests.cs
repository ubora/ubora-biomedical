using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
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
    }
}
