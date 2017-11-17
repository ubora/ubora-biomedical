using System;
using System.Collections.Generic;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Commands;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications.Commands
{
    public class AnswerDeviceClassificationCommandTests : IntegrationFixture
    {
        public readonly Guid _projectId = Guid.NewGuid();
        public readonly Guid _questionnaireId = Guid.NewGuid();
        private DeviceClass _deviceClass;

        [Fact]
        public void Last_Answer_Sets_Project_As_Finished()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => _.Create_Questionnaire_Aggregate())
                .When(_ => _.Answer("q1", "a1"))
                    .And(_ => _.Answer("q2", "a2"))
                .Then(_ => _.Questionnaire_Should_Be_Finished())
                .BDDfy();
        }

        private void Create_Questionnaire_Aggregate()
        {
            var questionnaireAggregate = new DeviceClassificationAggregate();

            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("a1", "q2"),
                }),
                new Question("q2", new[]
                {
                    new Answer("a2", null)
                })
            };

            var deviceClass = new DeviceClassTwoB().WithConditions(new []
            {
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q1", "a1" },
                    { "q2", "a2" }
                })
            });

            var testQuestionnaireTree = new DeviceClassificationQuestionnaireTree(questions, new[] { deviceClass });

            questionnaireAggregate.Set(x => x.QuestionnaireTree, testQuestionnaireTree);
            questionnaireAggregate.Set(x => x.ProjectId, _projectId);
            questionnaireAggregate.Set(x => x.Id, _questionnaireId);

            Session.Store(questionnaireAggregate);
            Session.SaveChanges();
        }

        private void Answer(string questionId, string answerId)
        {
            Processor.Execute(new AnswerDeviceClassificationCommand
            {
                QuestionId = questionId,
                AnswerId = answerId,
                ProjectId = _projectId,
                QuestionnaireId = _questionnaireId,
                Actor = new DummyUserInfo()
            });
        }

        private void Questionnaire_Should_Be_Finished()
        {
            var questionnaireAggregate = Session.Load<DeviceClassificationAggregate>(_questionnaireId);

            questionnaireAggregate.FinishedAt.Should().BeCloseTo(DateTime.UtcNow, precision: 200);
        }
    }
}
