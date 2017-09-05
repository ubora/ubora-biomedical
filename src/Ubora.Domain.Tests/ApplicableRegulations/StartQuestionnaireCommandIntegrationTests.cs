using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.ApplicableRegulations;
using Ubora.Domain.ApplicableRegulations.Commands;
using Xunit;

namespace Ubora.Domain.Tests.ApplicableRegulations
{
    public class StartQuestionnaireCommandIntegrationTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _questionnaireId = Guid.NewGuid();

        [Fact]
        public void Foo()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => Start_Questionnaire())
                .Then(_ => Assert_Questionnaire_Is_Initialized())
                .BDDfy();
        }

        private void Start_Questionnaire()
        {
            Processor.Execute(new StartQuestionnaireCommand
            {
                NewQuestionnaireId = _questionnaireId,
                ProjectId = _projectId,
                Actor = new DummyUserInfo()
            });
        }

        private void Assert_Questionnaire_Is_Initialized()
        {
            var projectQuestionnaireAggregate = Session.Events
                .AggregateStream<ProjectQuestionnaireAggregate>(_questionnaireId);

            projectQuestionnaireAggregate.ProjectId
                .Should().Be(_projectId);

            var expectedQuestionnaire = QuestionnaireFactory.Create(_questionnaireId);

            projectQuestionnaireAggregate.Questionnaire
                .ShouldBeEquivalentTo(expectedQuestionnaire);
        }
    }
}
