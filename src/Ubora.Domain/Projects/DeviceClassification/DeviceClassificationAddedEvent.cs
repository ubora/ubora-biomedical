using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class DeviceClassificationSavedEvent : UboraEvent
    {
        public Guid ProjectId { get; set; }
        public string DeviceClassification { get; set; }
        public Guid Id { get; set; }

        public DeviceClassificationSavedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription() => $"Saved {DeviceClassification} classification to project";
    }
}
