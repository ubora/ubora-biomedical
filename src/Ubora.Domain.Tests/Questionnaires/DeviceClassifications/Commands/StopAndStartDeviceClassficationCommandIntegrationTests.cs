using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications
{
    public class StopAndStartDeviceClassficationCommandIntegrationTests : IntegrationFixture
    {
        public readonly Guid ProjectId = Guid.NewGuid();
        public readonly Guid StopQuestionnaireId = Guid.NewGuid();
        public readonly Guid StartQuestionnaireId = Guid.NewGuid();

        [Fact]
        public void Questionnaire_Can_Be_Restarted()
        {
            this.Given(_ => this.Create_Project(ProjectId))
                    .And(_ => _.Start_Device_Classification_That_Will_Be_Stopped())
                .When(_ => _.Execute_Stop_And_Start_Device_Classification_Command())
                .Then(_ => _.Old_Questionnaire_Is_Stopped_And_New_Started())
                .BDDfy();
        }

        private void Start_Device_Classification_That_Will_Be_Stopped()
        {
            Processor.Execute(new StartClassifyingDeviceCommand
            {
                Id = StopQuestionnaireId,
                ProjectId = ProjectId,
                Actor = new DummyUserInfo()
            });
        }

        private void Execute_Stop_And_Start_Device_Classification_Command()
        {
            var result = Processor.Execute(new StopAndStartDeviceClassificationCommand
            {
                StopQuestionnaireId = StopQuestionnaireId,
                StartQuestionnaireId = StartQuestionnaireId,
                ProjectId = ProjectId,
                Actor = new DummyUserInfo()
            });
            result.IsSuccess.Should().BeTrue();
        }

        private void Old_Questionnaire_Is_Stopped_And_New_Started()
        {
            var stoppedQuestionnaire = Session.Load<DeviceClassificationAggregate>(StopQuestionnaireId);
            stoppedQuestionnaire.FinishedAt.Should().BeCloseTo(DateTime.UtcNow, 250);
            stoppedQuestionnaire.IsStopped.Should().BeTrue();

            var startedQuestionnaire = Session.Events.AggregateStream<DeviceClassificationAggregate>(StartQuestionnaireId);
            startedQuestionnaire.StartedAt.Should().BeCloseTo(DateTime.UtcNow, 250);
            startedQuestionnaire.ProjectId.Should().Be(ProjectId);
            startedQuestionnaire.QuestionnaireTree.ShouldBeEquivalentTo(new DeviceClassificationQuestionnaireTreeFactory().CreateDeviceClassification());
            startedQuestionnaire.IsFinished.Should().BeFalse();
        }
    }
}
