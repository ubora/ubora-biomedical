using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations.Events
{
    public class ApplicableRegulationsQuestionAnsweredEvent : UboraEvent
    {
        public ApplicableRegulationsQuestionAnsweredEvent(UserInfo initiatedBy, Guid questionnaireId, string questionId, string answerId, DateTime answeredAt) : base(initiatedBy)
        {
            QuestionnaireId = questionnaireId;
            QuestionId = questionId;
            AnswerId = answerId;
            AnsweredAt = answeredAt;
        }

        public Guid QuestionnaireId { get; private set; }
        public string QuestionId { get; private set; }
        public string AnswerId { get; private set; }
        public DateTime AnsweredAt { get; private set; }

        public override string GetDescription() => "answered a question.";
    }
}