using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Marten.Pagination;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Commands;
using Ubora.Domain.Projects.StructuredInformations.Specifications;
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
            var deviceStructuredInformations = new List<DeviceStructuredInformation>
            {
                deviceStructuredInformation
            };
            
            QueryProcessorMock.Setup(x => x.Find<DeviceStructuredInformation>(It.IsAny<IsWorkpackageTypeDeviceStructuredInformationSpec>()))
                .Returns(new PagedList<DeviceStructuredInformation>(deviceStructuredInformations.AsQueryable(), 1,10));

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

            result.ActionName.Should().Be(nameof(WorkpackageTwoController.StructuredInformationOnTheDevice));
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

        [Fact]
        public void EditHealthTechnologySpecifications_Executes_Command_When_Valid_Model()
        {
            var postModel = new HealthTechnologySpecificationsViewModel();
            var modelMapperMock = new Mock<HealthTechnologySpecificationsViewModel.Mapper>();
            var expectedExecutedCommand = new EditHealthTechnologySpecificationInformationCommand();

            modelMapperMock.Setup(m => m.MapToCommand(postModel))
                .Returns(expectedExecutedCommand);

            CommandProcessorMock
                .Setup(x => x.Execute(expectedExecutedCommand))
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)_controller.EditHealthTechnologySpecifications(postModel, modelMapperMock.Object, Mock.Of<HealthTechnologySpecificationsViewModel.Factory>());

            // Assert
            CommandProcessorMock
                .Verify(x => x.Execute(It.IsAny<EditHealthTechnologySpecificationInformationCommand>()), Times.Once);

            result.ActionName.Should().Be(nameof(WorkpackageTwoController.StructuredInformationOnTheDevice));
        }

        [Fact]
        public void EditHealthTechnologySpecifications_Does_Not_Execute_Command_When_Invalid_Model()
        {
            var expectedResult = Mock.Of<IActionResult>();
            var modelFactory = Mock.Of<HealthTechnologySpecificationsViewModel.Factory>();

            _controllerMock.Setup(c => c.HealthTechnologySpecifications(modelFactory))
                .Returns(expectedResult);

            var model = new HealthTechnologySpecificationsViewModel();
            var modelMapper = Mock.Of<HealthTechnologySpecificationsViewModel.Mapper>();

            _controller.ViewData.ModelState.AddModelError("", "error");

            // Act
            var result = _controller.EditHealthTechnologySpecifications(model, modelMapper, modelFactory);

            // Assert
            CommandProcessorMock
                .Verify(x => x.Execute(It.IsAny<EditHealthTechnologySpecificationInformationCommand>()), Times.Never);

            result.Should().BeSameAs(expectedResult);
        }
    }
}
