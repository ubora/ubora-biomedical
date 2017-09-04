using System;

namespace Ubora.Domain.ApplicableRegulations
{
    public class ProjectQuestionnaireAggregate
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Questionnaire Questionnaire { get; private set; }
        public DateTime? FinishedAt { get; private set; }

        private void Apply(QuestionnaireStartedEvent e)
        {
            if (e.NewQuestionnaireId == default(Guid)) { throw new InvalidOperationException(); }
            if (e.ProjectId == default(Guid)) { throw new InvalidOperationException(); }
            if (Questionnaire != null) { throw new InvalidOperationException(); }

            Id = e.NewQuestionnaireId;
            ProjectId = e.ProjectId;
            Questionnaire = QuestionnaireFactory.Create(e.NewQuestionnaireId);
        }

        private void Apply(QuestionAnsweredEvent e)
        {
            var question = Questionnaire.GetNextUnanswered();
            if (question.Id != e.QuestionId)
            {
                throw new InvalidOperationException();
            }

            question.Answer = e.Answer;

            if (Questionnaire.GetNextUnanswered() == null)
            {
                FinishedAt = DateTime.UtcNow;
            }
        }
    }
}