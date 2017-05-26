using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class DeviceClassificationSetEvent : UboraEvent
    {
        public string DeviceClassification { get; }
        public Guid Id { get; }

        public DeviceClassificationSetEvent(Guid id, string deviceClassification, UserInfo initiatedBy) : base(initiatedBy)
        {
            Id = id;
            DeviceClassification = deviceClassification;
        }

        public override string GetDescription() => $"Saved {DeviceClassification} classification to project";
    }
}
