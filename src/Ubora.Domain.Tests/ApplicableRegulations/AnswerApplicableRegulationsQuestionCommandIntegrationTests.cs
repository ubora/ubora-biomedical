using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.ApplicableRegulations;
using Ubora.Domain.ApplicableRegulations.Commands;
using Ubora.Domain.ApplicableRegulations.Events;
using Ubora.Domain.Projects._Events;
using Xunit;

namespace Ubora.Domain.Tests.ApplicableRegulations
{
    public class AnswerApplicableRegulationsQuestionCommandIntegrationTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _questionnaireId = Guid.NewGuid();
        private bool _expectedAnswer;
        private Guid _firstQuestionId;
        private Guid _secondQuestionId;

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Questionnaire_Questions_Can_Be_Answered(
            bool answer)
        {
            _expectedAnswer = answer;

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
            var questionnaireStartedEvent = new ApplicableRegulationsQuestionnaireStartedEvent(new DummyUserInfo(), _questionnaireId, _projectId);

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

        private void ExecuteAnswerQuestionCommand(Guid questionId)
        {
            var command = new AnswerApplicableRegulationsQuestionCommand
            {
                ProjectId = _projectId,
                QuestionnaireId = _questionnaireId,
                QuestionId = questionId,
                Answer = _expectedAnswer,
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

        public void Assert_Question_Is_Answered(Guid questionId)
        {
            var aggregate = Session.Load<ApplicableRegulationsQuestionnaireAggregate>(_questionnaireId);

            var question = aggregate.Questionnaire.FindQuestionOrThrow(questionId);

            question.Answer.Should().Be(_expectedAnswer);
        }
    }
}
