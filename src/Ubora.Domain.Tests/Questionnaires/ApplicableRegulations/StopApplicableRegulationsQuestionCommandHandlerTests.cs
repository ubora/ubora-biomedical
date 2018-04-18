using System;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.ApplicableRegulations
{
    public class StopApplicableRegulationsQuestionCommandHandlerTests
    {
        private readonly Mock<IDocumentSession> _documentSessionMock;
        private readonly StopApplicableRegulationsQuestionnaireCommand.Handler _handlerUnderTest;

        public StopApplicableRegulationsQuestionCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);
            _handlerUnderTest = new StopApplicableRegulationsQuestionnaireCommand.Handler(_documentSessionMock.Object);
        }

        [Fact]
        public void Returns_Failed_Result_When_Questionnaire_Is_Stopped()
        {
            var aggregate = new ApplicableRegulationsQuestionnaireAggregate();

            var command = new StopApplicableRegulationsQuestionnaireCommand
            {
                QuestionnaireId = new Guid()
            };

            _documentSessionMock.Setup(x => x.Load<ApplicableRegulationsQuestionnaireAggregate>(command.QuestionnaireId))
                .Returns(aggregate);

            // sets IsStopped true
            aggregate.Set(x => x.FinishedAt, DateTime.UtcNow);

            // Act
            var result = _handlerUnderTest.Handle(command);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
