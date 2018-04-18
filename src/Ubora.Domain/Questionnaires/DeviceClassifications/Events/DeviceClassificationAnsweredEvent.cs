using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.Events
{
    public class DeviceClassificationAnsweredEvent : ProjectEvent
    {
        public DeviceClassificationAnsweredEvent(UserInfo initiatedBy, Guid projectId, Guid questionnaireId, string questionId, string answerId, DateTime answeredAt) : base(initiatedBy, projectId)
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

        public override string GetDescription() => "answered a device classification question";
    }
}
