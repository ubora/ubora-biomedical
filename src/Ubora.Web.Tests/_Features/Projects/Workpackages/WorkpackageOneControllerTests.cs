using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageOneControllerTests : ProjectControllerTestsBase
    {
        private readonly WorkpackageOneController _workpackageOneController;
        private readonly Mock<ICommandQueryProcessor> _processorMock;
        private readonly Mock<IMapper> _mapperMock;

        public WorkpackageOneControllerTests()
        {
            _processorMock = new Mock<ICommandQueryProcessor>();
            _mapperMock = new Mock<IMapper>();
            _workpackageOneController = new WorkpackageOneController(_processorMock.Object, _mapperMock.Object)
            {
                Url = Mock.Of<IUrlHelper>()
            };
            SetProjectAndUserContext(_workpackageOneController);
        }

        [Fact]
        public void Returns_View_With_ModelState_Errors_When_Form_Post_Is_Not_Valid()
        {
            var stepId = Guid.NewGuid().ToString();
            var step = Mock.Of<WorkpackageStep>();
            var workpackageOne = Mock.Of<WorkpackageOne>(x => x.GetSingleStep(stepId) == step);

            _processorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageOne);

            var expectedModel = new EditStepViewModel();
            _mapperMock.Setup(m => m.Map<EditStepViewModel>(step))
                .Returns(expectedModel);

            _workpackageOneController.ModelState.AddModelError("", "testError");

            var postModel = new EditStepViewModel { StepId = stepId };

            // Act
            var result = (ViewResult)_workpackageOneController.Edit(postModel);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);

            _processorMock.Verify(x => x.Execute(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public void Returns_View_With_ModelState_Errors_When_Handling_Of_Command_Is_Not_Successful()
        {
            var stepId = Guid.NewGuid().ToString();
            var step = Mock.Of<WorkpackageStep>();
            var workpackageOne = Mock.Of<WorkpackageOne>(x => x.GetSingleStep(stepId) == step);

            _processorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageOne);

            var expectedModel = new EditStepViewModel();
            _mapperMock.Setup(m => m.Map<EditStepViewModel>(step))
                .Returns(expectedModel);

            _processorMock.Setup(x => x.Execute(It.IsAny<ICommand>())).Returns(new CommandResult("dummyError"));

            var postModel = new EditStepViewModel { StepId = stepId };

            // Act
            var result = (ViewResult)_workpackageOneController.Edit(postModel);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);
        }

        [Fact]
        public void Redirects_When_Command_Is_Executed_Successfully()
        {
            EditWorkpackageOneStepCommand executedCommand = null;
            _processorMock
                .Setup(x => x.Execute(It.IsAny<EditWorkpackageOneStepCommand>()))
                .Callback<EditWorkpackageOneStepCommand>(c => executedCommand = c)
                .Returns(new CommandResult());

            var stepId = Guid.NewGuid().ToString();
            var postModel = new EditStepViewModel
            {
                StepId = stepId,
                Content = "expectedValue"
            };

            // Act
            var result = (RedirectToActionResult)_workpackageOneController.Edit(postModel);

            // Assert
            executedCommand.StepId.Should().Be(stepId);
            executedCommand.ProjectId.Should().Be(ProjectId);
            executedCommand.NewValue.Should().Be("expectedValue");

            result.ActionName.Should().Be(nameof(WorkpackageOneController.Read));
        }

        [Fact]
        public void Returns_View_Without_Editing_For_Workpackage_One_Step()
        {
            var stepId = Guid.NewGuid().ToString();
            var step = Mock.Of<WorkpackageStep>();
            var workpackageOne = Mock.Of<WorkpackageOne>(x => x.GetSingleStep(stepId) == step);

            _processorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageOne);

            var expectedModel = new ReadStepViewModel();
            _mapperMock.Setup(m => m.Map<ReadStepViewModel>(step))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_workpackageOneController.Read(stepId);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);
        }

        [Fact]
        public void Returns_View_With_Editing_For_Workpackage_One_Step()
        {
            var stepId = Guid.NewGuid().ToString();
            var step = Mock.Of<WorkpackageStep>();
            var workpackageOne = Mock.Of<WorkpackageOne>(x => x.GetSingleStep(stepId) == step);

            _processorMock.Setup(x => x.FindById<WorkpackageOne>(ProjectId))
                .Returns(workpackageOne);

            var expectedModel = new ReadStepViewModel();
            _mapperMock.Setup(m => m.Map<ReadStepViewModel>(step))
                .Returns(expectedModel);

            // Act
            var result = (ViewResult)_workpackageOneController.Read(stepId);

            // Assert
            result.Model.Should().BeSameAs(expectedModel);
        }
    }
}
