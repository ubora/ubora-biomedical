using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.ApplicableRegulations.Events
{
    public class ApplicableRegulationsQuestionAnsweredEvent : UboraEvent
    {
        public ApplicableRegulationsQuestionAnsweredEvent(UserInfo initiatedBy, Guid questionId, bool answer) : base(initiatedBy)
        {
            QuestionId = questionId;
            Answer = answer;
        }

        public Guid QuestionId { get; private set; }
        public bool Answer { get; private set; }

        public override string GetDescription() => "answered a question.";
    }
}