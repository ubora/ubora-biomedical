using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Commands;
using Ubora.Web._Features.Projects.DeviceClassifications;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.DeviceClassifications
{
    public class DeviceClassificationsControllerTests : ProjectControllerTestsBase
    {
        private readonly DeviceClassificationsController _controller;
        private readonly Mock<DeviceClassificationsController> _controllerMock;

        public DeviceClassificationsControllerTests()
        {
            _controllerMock = new Mock<DeviceClassificationsController> { CallBase = true };
            _controller = _controllerMock.Object;
            SetUpForTest(_controller);
        }

        [Fact]
        public void Index_Returns_View_With_Model()
        {
            var modelFactoryMock = new Mock<DeviceClassificationIndexViewModel.Factory>();
            var expectedModel = new DeviceClassificationIndexViewModel();

            modelFactoryMock.Setup(x => x.Create(this.ProjectId))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_controller.Index(modelFactoryMock.Object);

            // Assert
            result.ViewName.Should().Be("DeviceClassificationIndex");
        }

        [Fact]
        public void Start_Executes_Command_For_Device_Classification_And_Redirects_To_First_Question()
        {
            StartClassifyingDeviceCommand executedCommand = null;

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<StartClassifyingDeviceCommand>()))
                .Callback<StartClassifyingDeviceCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var modelFactory = Mock.Of<DeviceClassificationIndexViewModel.Factory>();

            // Act
            var result = (RedirectToActionResult)_controller.Start(modelFactory);

            // Assert
            executedCommand.Id.Should().NotBe(default(Guid)); // Id should be unique.

            result.ActionName.Should().Be(nameof(DeviceClassificationsController.Current));
            result.RouteValues["questionnaireId"].Should().Be(executedCommand.Id);
        }

        [Fact]
        public void Start_Returns_Index_With_Errors_When_Command_Execution_Is_Unsuccessful()
        {
            StartClassifyingDeviceCommand executedCommand = null;

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<StartClassifyingDeviceCommand>()))
                .Callback<StartClassifyingDeviceCommand>(c => executedCommand = c)
                .Returns(CommandResult.Failed("error"));

            var modelFactory = Mock.Of<DeviceClassificationIndexViewModel.Factory>();

            var expectedActionResult = Mock.Of<IActionResult>();
            _controllerMock.Setup(x => x.Index(modelFactory))
                .Returns(expectedActionResult);

            // Act
            var result = _controller.Index(modelFactory);

            // Assert
            result.Should().Be(expectedActionResult);
        }
    }
}
