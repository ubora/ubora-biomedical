using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Commands;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageTwoControllerTests : ProjectControllerTestsBase
    {
        private readonly WorkpackageTwoController _controller;
        private readonly Mock<WorkpackageTwoController> _controllerMock;

        public WorkpackageTwoControllerTests()
        {
            _controllerMock = new Mock<WorkpackageTwoController> { CallBase = true };
            _controller = _controllerMock.Object;
            SetUpForTest(_controller);
        }

        [Fact]
        public void UserAndEnvironment_Returns_Form_View_With_Mapped_Values_From_Domain()
        {
            var deviceStructuredInformation = new DeviceStructuredInformation();
            QueryProcessorMock.Setup(x => x.FindById<DeviceStructuredInformation>(ProjectId))
                .Returns(deviceStructuredInformation);

            var expectedModel = new UserAndEnvironmentInformationViewModel();

            var modelFactoryMock = new Mock<UserAndEnvironmentInformationViewModel.Factory>();
            modelFactoryMock.Setup(x => x.Create(deviceStructuredInformation.UserAndEnvironment))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult) _controller.UserAndEnvironment(modelFactoryMock.Object);

            // Assert
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }

        [Fact]
        public void EditUserAndEnvironment_Executes_Command_When_Valid_Model()
        {
            var postModel = new UserAndEnvironmentInformationViewModel();
            var modelMapperMock = new Mock<UserAndEnvironmentInformationViewModel.Mapper>();
            var expectedExecutedCommand = new EditUserAndEnvironmentInformationCommand();

            modelMapperMock.Setup(m => m.MapToCommand(postModel))
                .Returns(expectedExecutedCommand);

            CommandProcessorMock
                .Setup(x => x.Execute(expectedExecutedCommand))
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)_controller.EditUserAndEnvironment(postModel, modelMapperMock.Object, Mock.Of<UserAndEnvironmentInformationViewModel.Factory>());

            // Assert
            CommandProcessorMock
                .Verify(x => x.Execute(It.IsAny<EditUserAndEnvironmentInformationCommand>()), Times.Once);

            result.ActionName.Should().Be(nameof(WorkpackageTwoController.StructuredInformationOnTheDeviceResult));
        }

        [Fact]
        public void EditUserAndEnvironment_Does_Not_Execute_Command_When_Invalid_Model()
        {
            var expectedResult = Mock.Of<IActionResult>();
            var modelFactory = Mock.Of<UserAndEnvironmentInformationViewModel.Factory>();

            _controllerMock.Setup(c => c.UserAndEnvironment(modelFactory))
                .Returns(expectedResult);

            var model = new UserAndEnvironmentInformationViewModel();
            var modelMapper = Mock.Of<UserAndEnvironmentInformationViewModel.Mapper>();

            _controller.ViewData.ModelState.AddModelError("", "error");

            // Act
            var result = _controller.EditUserAndEnvironment(model, modelMapper, modelFactory);

            // Assert
            CommandProcessorMock
                .Verify(x => x.Execute(It.IsAny<EditUserAndEnvironmentInformationCommand>()), Times.Never);

            result.Should().BeSameAs(expectedResult);
        }
    }
}
