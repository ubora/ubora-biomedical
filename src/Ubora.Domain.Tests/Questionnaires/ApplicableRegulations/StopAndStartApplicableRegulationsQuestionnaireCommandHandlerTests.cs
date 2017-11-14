using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.ApplicableRegulations
{
    public class StopAndStartApplicableRegulationsQuestionnaireCommandHandlerTests
    {
        private Mock<IDocumentSession> _documentSessionMock;
        private StopAndStartApplicableRegulationsQuestionnaireCommand.Handler _handlerUnderTest;

        public StopAndStartApplicableRegulationsQuestionnaireCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);
            _handlerUnderTest = new StopAndStartApplicableRegulationsQuestionnaireCommand.Handler(_documentSessionMock.Object);
        }

        [Fact]
        public void Returns_Failed_Result_When_Questionnaire_Is_Stopped()
        {
            var aggregate = new ApplicableRegulationsQuestionnaireAggregate();
            var command = new StopAndStartApplicableRegulationsQuestionnaireCommand
            {
                QuestionnaireId = Guid.NewGuid()
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
