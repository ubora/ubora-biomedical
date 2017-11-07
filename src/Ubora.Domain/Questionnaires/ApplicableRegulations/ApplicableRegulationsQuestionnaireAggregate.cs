using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations
{
    public class ApplicableRegulationsQuestionnaireAggregate : Entity<ApplicableRegulationsQuestionnaireAggregate>
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public ApplicableRegulationsQuestionnaireTree Questionnaire { get; private set; }
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

            question.ChooseAnswer(e.AnswerId, e.AnsweredAt);

            if (Questionnaire.FindNextUnansweredQuestion() == null)
            {
                FinishedAt = e.AnsweredAt;
            }
        }

        private void Apply(ApplicableRegulationsQuestionnaireStoppedEvent e)
        {
            if (IsFinished) { throw new InvalidOperationException(); }

            FinishedAt = e.StoppedAt;
        }
    }
}