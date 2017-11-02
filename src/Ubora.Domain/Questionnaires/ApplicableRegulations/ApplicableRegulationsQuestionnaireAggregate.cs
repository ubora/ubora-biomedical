using System;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations
{
    public class ApplicableRegulationsQuestionnaireAggregate
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public QuestionnaireTree Questionnaire { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        public bool IsFinished => FinishedAt.HasValue;

        private void Apply(ApplicableRegulationsQuestionnaireStartedEvent e)
        {
            if (e.NewQuestionnaireId == default(Guid)) { throw new InvalidOperationException(); }
            if (e.ProjectId == default(Guid)) { throw new InvalidOperationException(); }
            if (Questionnaire != null) { throw new InvalidOperationException(); }

            Id = e.NewQuestionnaireId;
            ProjectId = e.ProjectId;
            Questionnaire = e.QuestionnaireTree;
            StartedAt = e.StartedAt;
        }

        private void Apply(ApplicableRegulationsQuestionAnsweredEvent e)
        {
            var question = Questionnaire.FindNextUnansweredQuestion();
            if (question.Id != e.QuestionId)
            {
                throw new InvalidOperationException();
            }

            question.ChooseAnswer(e.AnswerId);

            if (Questionnaire.FindNextUnansweredQuestion() == null)
            {
                FinishedAt = e.AnsweredAt;
            }
        }
    }
}