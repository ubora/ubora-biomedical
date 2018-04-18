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
    public class StopApplicableRegulationsQuestionnaireCommandIntegrationTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _questionnaireId = Guid.NewGuid();

        [Fact]
        public void Applicable_Regulations_Questionnaire_Can_Be_Stopped()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .And(_ => _.Start_Questionnaire())
                .When(_ => _.Stop_Questionnaire())
                .Then(_ => _.Questionnaire_Is_Stopped())
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

        private void Stop_Questionnaire()
        {
            var result = Processor.Execute(new StopApplicableRegulationsQuestionnaireCommand
            {
                QuestionnaireId = _questionnaireId,
                ProjectId = _projectId,
                Actor = new DummyUserInfo()

            });
            result.IsSuccess.Should().BeTrue();
        }

        private void Questionnaire_Is_Stopped()
        {
            var questionnaire = Session.Load<ApplicableRegulationsQuestionnaireAggregate>(_questionnaireId);
            questionnaire.IsStopped.Should().BeTrue();
            questionnaire.ProjectId.Should().Be(_projectId);
        }
    }
}
