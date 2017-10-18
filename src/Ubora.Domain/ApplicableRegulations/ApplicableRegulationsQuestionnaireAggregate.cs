using System;
using Ubora.Domain.ApplicableRegulations.Events;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.ApplicableRegulations
{
    public class ApplicableRegulationsQuestionnaireAggregate : Entity<ApplicableRegulationsQuestionnaireAggregate>
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Questionnaire Questionnaire { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        public bool IsFinished => FinishedAt.HasValue;
        public bool IsStopped => FinishedAt.HasValue && Questionnaire.FindNextUnansweredQuestion() != null;
      

        private void Apply(ApplicableRegulationsQuestionnaireStartedEvent e)
        {
            if (e.NewQuestionnaireId == default(Guid)) { throw new InvalidOperationException(); }
            if (e.ProjectId == default(Guid)) { throw new InvalidOperationException(); }
            if (Questionnaire != null) { throw new InvalidOperationException(); }

            Id = e.NewQuestionnaireId;
            ProjectId = e.ProjectId;
            StartedAt = DateTime.UtcNow;
            Questionnaire = QuestionnaireFactory.Create();
        }

        private void Apply(ApplicableRegulationsQuestionAnsweredEvent e)
        {
            var question = Questionnaire.FindNextUnansweredQuestion();
            if (question.Id != e.QuestionId)
            {
                throw new InvalidOperationException();
            }

            question.AnswerQuestion(e.Answer);

            if (Questionnaire.FindNextUnansweredQuestion() == null)
            {
                FinishedAt = DateTime.UtcNow;
            }
        }

        private void Apply(ApplicableRegulationsQuestionnaireStoppedEvent e)
        {
            if (IsFinished) { throw new InvalidOperationException(); }

            FinishedAt = DateTime.UtcNow;
            
        }
    }
}