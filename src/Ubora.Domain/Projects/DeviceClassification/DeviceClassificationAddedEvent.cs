using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class DeviceClassificationSetEvent : UboraEvent
    {
        public Classification NewClassification { get; }
        public Classification CurrentClassification { get; }
        public Guid Id { get; }

        public DeviceClassificationSetEvent(Guid id, Classification newClassification, Classification currentClassification, UserInfo initiatedBy) : base(initiatedBy)
        {
            Id = id;
            NewClassification = newClassification;
            CurrentClassification = currentClassification;
        }

        public override string GetDescription() => $"Saved new classification to project. New '{NewClassification.Text}', Current {CurrentClassification.Text}";
    }
}
