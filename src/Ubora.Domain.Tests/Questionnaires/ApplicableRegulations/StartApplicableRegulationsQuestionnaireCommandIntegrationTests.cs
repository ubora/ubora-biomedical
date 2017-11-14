using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.ApplicableRegulations
{
    public class StartApplicableRegulationsQuestionnaireCommandIntegrationTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _questionnaireId = Guid.NewGuid();

        [Fact]
        public void Applicable_Regulations_Questionnaire_Can_Be_Started()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => Start_Questionnaire())
                .Then(_ => Assert_Questionnaire_Is_Initialized())
                .BDDfy();
        }

        private void Start_Questionnaire()
        {
            Processor.Execute(new StartApplicableRegulationsQuestionnaireCommand
            {
                NewQuestionnaireId = _questionnaireId,
                ProjectId = _projectId,
                Actor = new DummyUserInfo()
            });
        }

        private void Assert_Questionnaire_Is_Initialized()
        {
            var projectQuestionnaireAggregate = Session.Events
                .AggregateStream<ApplicableRegulationsQuestionnaireAggregate>(_questionnaireId);

            projectQuestionnaireAggregate.ProjectId
                .Should().Be(_projectId);

            var actualQuestions = projectQuestionnaireAggregate.Questionnaire
                .Questions.ToList();

            var expectedQuestions = Domain.Questionnaires.ApplicableRegulations.ApplicableRegulationsQuestionnaireTreeFactory.Create()
                .Questions.ToList();

            actualQuestions.Count.Should().Be(expectedQuestions.Count);

            // Assert all resource names from the expected list are represented in the actual list. Not a perfect assert...
            actualQuestions.All(x => expectedQuestions.Any(eQ => eQ.Id == x.Id))
                .Should().BeTrue();
        }
    }
}
