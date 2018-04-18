using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.Events
{
    public class DeviceClassificationStoppedEvent : ProjectEvent
    {
        public DeviceClassificationStoppedEvent(UserInfo initiatedBy, Guid projectId, DateTime stoppedAt) : base(initiatedBy, projectId)
        {
            StoppedAt = stoppedAt;
        }

        public DateTime StoppedAt { get; private set; }

        public override string GetDescription() => "stopped the device classification questionnaire.";
    }
}
