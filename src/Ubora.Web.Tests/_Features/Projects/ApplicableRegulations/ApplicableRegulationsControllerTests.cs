using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.ApplicableRegulations.Commands;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Web._Features.Projects.ApplicableRegulations;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.ApplicableRegulations
{
    public class ApplicableRegulationsControllerTests : ProjectControllerTestsBase
    {
        private readonly Mock<ApplicableRegulationsController> _controllerMock;
        private ApplicableRegulationsController _controller => _controllerMock.Object;

        public ApplicableRegulationsControllerTests()
        {
            _controllerMock = new Mock<ApplicableRegulationsController> { CallBase = true };

            SetUpForTest(_controller);
        }

        [Fact]
        public void Start_Executes_Command_With_Success()
        {
            StartApplicableRegulationsQuestionnaireCommand executedCommand = null;

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<StartApplicableRegulationsQuestionnaireCommand>()))
                .Callback<StartApplicableRegulationsQuestionnaireCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            // Act
            var result = (RedirectToActionResult)_controller.Start(modelFactory: null);

            // Assert
            executedCommand.NewQuestionnaireId.Should().NotBe(default(Guid));

            result.ActionName.Should().Be(nameof(_controller.Next));

            result.RouteValues["questionnaireId"].Should().Be(executedCommand.NewQuestionnaireId);
        }

        [Fact]
        public void Start_Executes_Command_With_Failure()
        {
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<StartApplicableRegulationsQuestionnaireCommand>()))
                .Returns(CommandResult.Failed(""));

            var indexModelFactory = Mock.Of<QuestionnaireIndexViewModel.Factory>();

            var expectedResult = Mock.Of<IActionResult>();
            _controllerMock.Setup(x => x.Index(indexModelFactory))
                .Returns(expectedResult);

            // Act
            var result = _controller.Start(indexModelFactory);

            // Assert
            result.Should().BeSameAs(expectedResult);
            _controller.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public void AnswerYes_Does_Not_Execute_Command_When_Model_Is_Invalid()
        {
            _controller.ModelState.AddModelError("", "dummyError");

            var model = new NextQuestionViewModel
            {
                QuestionnaireId = Guid.NewGuid()
            };

            var expectedResult = Mock.Of<IActionResult>();
            _controllerMock.Setup(x => x.Next(model.QuestionnaireId))
                .Returns(expectedResult);

            // Act
            var result = _controller.AnswerYes(model);

            // Assert
            result.Should().BeSameAs(expectedResult);
            AssertZeroCommandsExecuted();
        }

        [Fact]
        public void AnswerYes_Executes_Command_With_Success()
        {
            AnswerApplicableRegulationsQuestionCommand executedCommand = null;

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<AnswerApplicableRegulationsQuestionCommand>()))
                .Callback<AnswerApplicableRegulationsQuestionCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var model = new NextQuestionViewModel
            {
                Id = Guid.NewGuid(),
                QuestionnaireId = Guid.NewGuid()
            };

            // Act
            var result = (RedirectToActionResult)_controller.AnswerYes(model);

            // Assert
            executedCommand.Answer.Should().BeTrue();
            executedCommand.QuestionId.Should().Be(model.Id);
            executedCommand.QuestionnaireId.Should().Be(model.QuestionnaireId);

            result.ActionName.Should().Be(nameof(_controller.Next));

            result.RouteValues["questionnaireId"].Should().Be(model.QuestionnaireId);
        }

        [Fact]
        public void AnswerYes_Executes_Command_With_Failure()
        {
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<AnswerApplicableRegulationsQuestionCommand>()))
                .Returns(CommandResult.Failed(""));

            var model = new NextQuestionViewModel
            {
                QuestionnaireId = Guid.NewGuid()
            };

            var expectedResult = Mock.Of<IActionResult>();
            _controllerMock.Setup(x => x.Next(model.QuestionnaireId))
                .Returns(expectedResult);

            // Act
            var result = _controller.AnswerYes(model);

            // Assert
            result.Should().BeSameAs(expectedResult);
            _controller.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public void AnswerNo_Does_Not_Execute_Command_When_Model_Is_Invalid()
        {
            _controller.ModelState.AddModelError("", "dummyError");

            var model = new NextQuestionViewModel
            {
                QuestionnaireId = Guid.NewGuid()
            };

            var expectedResult = Mock.Of<IActionResult>();
            _controllerMock.Setup(x => x.Next(model.QuestionnaireId))
                .Returns(expectedResult);

            // Act
            var result = _controller.AnswerNo(model);

            // Assert
            result.Should().BeSameAs(expectedResult);
            AssertZeroCommandsExecuted();
        }

        [Fact]
        public void AnswerNo_Executes_Command_With_Success()
        {
            AnswerApplicableRegulationsQuestionCommand executedCommand = null;

            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<AnswerApplicableRegulationsQuestionCommand>()))
                .Callback<AnswerApplicableRegulationsQuestionCommand>(c => executedCommand = c)
                .Returns(CommandResult.Success);

            var model = new NextQuestionViewModel
            {
                Id = Guid.NewGuid(),
                QuestionnaireId = Guid.NewGuid()
            };

            // Act
            var result = (RedirectToActionResult)_controller.AnswerNo(model);

            // Assert
            executedCommand.Answer.Should().BeFalse();
            executedCommand.QuestionId.Should().Be(model.Id);
            executedCommand.QuestionnaireId.Should().Be(model.QuestionnaireId);

            result.ActionName.Should().Be(nameof(_controller.Next));

            result.RouteValues["questionnaireId"].Should().Be(model.QuestionnaireId);
        }

        [Fact]
        public void AnswerNo_Executes_Command_With_Failure()
        {
            CommandProcessorMock.Setup(x => x.Execute(It.IsAny<AnswerApplicableRegulationsQuestionCommand>()))
                .Returns(CommandResult.Failed(""));

            var model = new NextQuestionViewModel
            {
                QuestionnaireId = Guid.NewGuid()
            };

            var expectedResult = Mock.Of<IActionResult>();
            _controllerMock.Setup(x => x.Next(model.QuestionnaireId))
                .Returns(expectedResult);

            // Act
            var result = _controller.AnswerNo(model);

            // Assert
            result.Should().BeSameAs(expectedResult);
            _controller.ModelState.IsValid.Should().BeFalse();
        }
    }
}
