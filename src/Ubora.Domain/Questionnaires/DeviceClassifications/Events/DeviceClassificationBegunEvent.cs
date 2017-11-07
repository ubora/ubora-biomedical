using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.Events
{
    public class DeviceClassificationStartedEvent : ProjectEvent
    {
        public DeviceClassificationStartedEvent(UserInfo initiatedBy, Guid projectId, Guid id, DateTime startedAt, DeviceClassificationQuestionnaireTree questionnaireTree) : base(initiatedBy, projectId)
        {
            Id = id;
            StartedAt = startedAt;
            QuestionnaireTree = questionnaireTree;
        }

        public Guid Id { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DeviceClassificationQuestionnaireTree QuestionnaireTree { get; private set; }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}
