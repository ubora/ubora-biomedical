using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class EditedProjectDeviceClassificationEvent : UboraEvent
    {
        public Classification NewClassification { get; }
        public Classification CurrentClassification { get; }
        public Guid Id { get; }

        public EditedProjectDeviceClassificationEvent(Guid id, Classification newClassification, Classification currentClassification, UserInfo initiatedBy) : base(initiatedBy)
        {
            Id = id;
            NewClassification = newClassification;
            CurrentClassification = currentClassification;
        }

        public override string GetDescription()
        {
            if (CurrentClassification == null)
            {
                return $"saved classification to project. Classification is '{NewClassification.Text}'";
            }

            return $"saved new classification to project. New '{NewClassification.Text}', Current {CurrentClassification.Text}";
        }
    }
}
