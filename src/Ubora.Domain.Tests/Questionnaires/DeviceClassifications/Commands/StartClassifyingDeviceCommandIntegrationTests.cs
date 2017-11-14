using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications.Commands
{
    public class StartClassifyingDeviceCommandIntegrationTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _questionnaireId1 = Guid.NewGuid();
        private readonly Guid _questionnaireId2 = Guid.NewGuid();

        [Fact]
        public void Only_One_Device_Classification_Questionnaire_Can_Be_Started_At_Once()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => Start_Questionnaire(_questionnaireId1))
                .When(_ => Start_Questionnaire(_questionnaireId2))
                .Then(_ => Assert_Only_One_Questionnaire_Started())
                .BDDfy();
        }

        private void Start_Questionnaire(Guid questionnaireId)
        {
            Processor.Execute(new StartClassifyingDeviceCommand
            {
                Id = questionnaireId,
                ProjectId = _projectId,
                Actor = new DummyUserInfo()
            });
        }

        private void Assert_Only_One_Questionnaire_Started()
        {
            var onlyQuestionnaire = Session.Query<DeviceClassificationAggregate>().Single();

            onlyQuestionnaire.Id.Should().Be(_questionnaireId1);
            onlyQuestionnaire.QuestionnaireTreeVersion.Should().Be(1);
        }
    }
}
