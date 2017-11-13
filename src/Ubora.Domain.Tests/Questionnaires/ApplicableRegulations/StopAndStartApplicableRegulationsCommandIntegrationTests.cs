using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.ApplicableRegulations
{
    public class StopAndStartApplicableRegulationCommandIntegrationTests : IntegrationFixture
    {
        public readonly Guid Projectid = Guid.NewGuid();
        public readonly Guid StopQuestionnaireId = Guid.NewGuid();
        public readonly Guid NewQuestionnaireId = Guid.NewGuid();

        [Fact]
        public void Applicable_Regulation_Questionnaire_Can_Be_Restarted()
        {
            this.Given(_ => this.Create_Project(Projectid))
                    .And(_ => _.Start_Applicable_Regulation_Questionnaire_That_Will_Be_Stopped())
                .When(_ => _.Execute_Stop_And_Start_Applicable_Regulation_Questionnaire_Command())
                .Then(_ => _.Old_Questionnaire_Is_Stopped_And_New_Started())
                .BDDfy();
        }

        private void Start_Applicable_Regulation_Questionnaire_That_Will_Be_Stopped()
        {
            Processor.Execute(new StartApplicableRegulationsQuestionnaireCommand
            {
                ProjectId = Projectid,
                NewQuestionnaireId = StopQuestionnaireId,
                Actor = new DummyUserInfo()
            });
        }

        private void Execute_Stop_And_Start_Applicable_Regulation_Questionnaire_Command()
        {
            var result = Processor.Execute(new StopAndStartApplicableRegulationsQuestionnaireCommand
            {
                ProjectId = Projectid,
                QuestionnaireId = StopQuestionnaireId,
                Actor = new DummyUserInfo(),
                NewQuestionnaireId = NewQuestionnaireId
            });
            result.IsSuccess.Should().BeTrue();
        }

        private void Old_Questionnaire_Is_Stopped_And_New_Started()
        {
            var stoppedQuestionnaire =
                Session.Load<ApplicableRegulationsQuestionnaireAggregate>(StopQuestionnaireId);
            stoppedQuestionnaire.FinishedAt.Should().BeCloseTo(DateTime.UtcNow, 250);
            stoppedQuestionnaire.IsStopped.Should().BeTrue();

            var startedQuestionnaire =
                Session.Events.AggregateStream<ApplicableRegulationsQuestionnaireAggregate>(NewQuestionnaireId);
            startedQuestionnaire.StartedAt.Should().BeCloseTo(DateTime.UtcNow, 250);
            startedQuestionnaire.ProjectId.Should().Be(Projectid);
            startedQuestionnaire.Questionnaire.ShouldBeEquivalentTo(ApplicableRegulationsQuestionnaireTreeFactory.Create());
            startedQuestionnaire.IsFinished.Should().BeFalse();
        }

    }
}
