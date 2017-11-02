using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Commands;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Events;
using Xunit;

namespace Ubora.Domain.Tests.ApplicableRegulations
{
    public class AnswerApplicableRegulationsQuestionCommandIntegrationTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _questionnaireId = Guid.NewGuid();
        private string _expectedAnswerId;
        private string _firstQuestionId;
        private string _secondQuestionId;

        [Theory]
        [InlineData("a1")]
        [InlineData("a2")]
        public void Questionnaire_Questions_Can_Be_Answered(
            string answerId)
        {
            _expectedAnswerId = answerId;

            this.Given(_ => Create_Project_And_Applicable_Regulations_Questionnaire())
                .When(_ => Answer_First_Question())
                    .And(_ => Answer_Second_Question())
                .Then(_ => Assert_Question_Is_Answered(_firstQuestionId))
                    .And(_ => Assert_Question_Is_Answered(_secondQuestionId))
                .BDDfy();
        }

        private void Create_Project_And_Applicable_Regulations_Questionnaire()
        {
            var projectCreatedEvent = new ProjectCreatedEvent(
                initiatedBy: new DummyUserInfo(), 
                projectId: _projectId,
                title: "",
                clinicalNeed: "",
                areaOfUsage: "",
                potentialTechnology: "",
                gmdn: "");
            var questionnaireStartedEvent = new ApplicableRegulationsQuestionnaireStartedEvent(new DummyUserInfo(), _questionnaireId, _projectId, QuestionnaireTreeFactory.Create(), DateTime.UtcNow);

            Session.Events.Append(_projectId, projectCreatedEvent);
            Session.SaveChanges();
            Session.Events.Append(_questionnaireId, questionnaireStartedEvent);
            Session.SaveChanges();
        }

        private void Answer_First_Question()
        {
            _firstQuestionId = Processor.FindById<ApplicableRegulationsQuestionnaireAggregate>(_questionnaireId)
                .Questionnaire
                .FindNextUnansweredQuestion()
                .Id;

            ExecuteAnswerQuestionCommand(_firstQuestionId);
        }

        private void ExecuteAnswerQuestionCommand(string questionId)
        {
            var command = new AnswerApplicableRegulationsQuestionCommand
            {
                ProjectId = _projectId,
                QuestionnaireId = _questionnaireId,
                QuestionId = questionId,
                AnswerId = _expectedAnswerId,
                Actor = new DummyUserInfo()
            };
            Processor.Execute(command);
        }

        private void Answer_Second_Question()
        {
            _secondQuestionId = Processor.FindById<ApplicableRegulationsQuestionnaireAggregate>(_questionnaireId)
                .Questionnaire
                .FindNextUnansweredQuestion()
                .Id;

            ExecuteAnswerQuestionCommand(_secondQuestionId);
        }

        public void Assert_Question_Is_Answered(string questionId)
        {
            var aggregate = Session.Load<ApplicableRegulationsQuestionnaireAggregate>(_questionnaireId);

            var question = aggregate.Questionnaire.FindQuestionOrThrow(questionId);

            question.ChosenAnswer.Id.Should().Be(_expectedAnswerId);
        }
    }
}
