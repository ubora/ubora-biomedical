using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Web._Features.Projects.Dashboard;
using Ubora.Web.Infrastructure;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Dashboard
{
    public class DashboardControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<ICommandQueryProcessor> _processorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IStorageProvider> _storageProviderMock;
        private readonly Mock<ImageResizer> _imageResizerMock;
        private readonly DashboardController _dashboardController;

        public DashboardControllerTests()
        {
            _processorMock = new Mock<ICommandQueryProcessor>();
            _mapperMock = new Mock<IMapper>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _storageProviderMock = new Mock<IStorageProvider>();
            _imageResizerMock = new Mock<ImageResizer>();

            _dashboardController = new DashboardController(
                _processorMock.Object,
                _mapperMock.Object,
                _authorizationServiceMock.Object,
                _storageProviderMock.Object,
                _imageResizerMock.Object);

            SetProjectAndUserContext(_dashboardController);
        }

        [Fact]
        public async Task EditProjectImage_Saves_New_Image_And_Creates_New_Resized_Ones_And_Redirects_To_Dashboard()
        {
            _processorMock.Setup(x => x.Execute(It.IsAny<UpdateProjectImageCommand>()))
                .Returns(new CommandResult());

            var fileMock = Mock.Of<IFormFile>(x => x.FileName == "image.jpg");
            var model = new EditProjectImageViewModel { Image = fileMock };

            // Act
            var result = await _dashboardController.EditProjectImage(model);

            // Assert
            var expectedContainerName = BlobLocation.ContainerNames.Projects;
            var expectedBlobName = $"{ProjectId}/project-image/";

            _imageResizerMock.Verify(x => x.SaveAsJpegAsync(It.Is<BlobLocation>(y => y.ContainerName == expectedContainerName && y.BlobName == expectedBlobName + "original.jpg"), null));
            _imageResizerMock.Verify(x => x.CreateResizedImageAndSaveAsJpegAsync(It.Is<BlobLocation>(y => y.ContainerName == expectedContainerName && y.BlobName == expectedBlobName + "400x150.jpg"), null, 400, 150));
            _imageResizerMock.Verify(x => x.CreateResizedImageAndSaveAsJpegAsync(It.Is<BlobLocation>(y => y.ContainerName == expectedContainerName && y.BlobName == expectedBlobName + "1500x300.jpg"), null, 1500, 300));

            var actionResult = (RedirectToActionResult)result;
            actionResult.ActionName.Should().Be(nameof(DashboardController.Dashboard));
        }

        [Fact]
        public async Task EditProjectImage_Redirects_To_View_If_Modelstate_Has_Error()
        {
            _processorMock.Setup(x => x.Execute(It.IsAny<UpdateProjectImageCommand>()))
                .Returns(new CommandResult("Error"));

            var fileMock = Mock.Of<IFormFile>(x => x.FileName == "image.jpg");
            var model = new EditProjectImageViewModel { Image = fileMock };

            // Act
            var result = await _dashboardController.EditProjectImage(model);

            // Assert
            ModelState.ErrorCount.Should().Be(1);
        }

        [Fact]
        public async Task RemoveProjectImage_Removes_Image()
        {
            _processorMock.Setup(x => x.Execute(It.IsAny<DeleteProjectImageCommand>()))
                .Returns(new CommandResult());

            var model = new RemoveProjectImageViewModel();

            // Act
            var result = await _dashboardController.RemoveProjectImage(model);

            // Assert
            var expectedContainerName = BlobLocation.ContainerNames.Projects;
            var expectedBlobName = $"{ProjectId}/project-image/";

            _storageProviderMock.Verify(x => x.DeleteBlobAsync(expectedContainerName, expectedBlobName + "original.jpg"));
            _storageProviderMock.Verify(x => x.DeleteBlobAsync(expectedContainerName, expectedBlobName + "400x150.jpg"));
            _storageProviderMock.Verify(x => x.DeleteBlobAsync(expectedContainerName, expectedBlobName + "1500x300.jpg"));

            var actionResult = (RedirectToActionResult)result;
            actionResult.ActionName.Should().Be(nameof(DashboardController.Dashboard));
        }

        [Fact]
        public async Task RemoveProjectImage_Redirects_To_View_If_ModelState_Has_Errors()
        {
            _processorMock.Setup(x => x.Execute(It.IsAny<DeleteProjectImageCommand>()))
                .Returns(new CommandResult("Error"));
            _processorMock.Setup(x => x.FindById<Project>(ProjectId))
                .Returns(new Project());

            var model = new RemoveProjectImageViewModel();

            // Act
            var result = await _dashboardController.RemoveProjectImage(model);

            // Assert
            ModelState.ErrorCount.Should().Be(1);
        }
    }
}
