using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.DeviceClassification.Events
{
    public class EditedProjectDeviceClassificationEvent : ProjectEvent
    {
        public EditedProjectDeviceClassificationEvent(UserInfo initiatedBy, Guid projectId, Classification newClassification, Classification currentClassification) : base(initiatedBy, projectId)
        {
            NewClassification = newClassification;
            CurrentClassification = currentClassification;
        }

        public Classification NewClassification { get; }
        public Classification CurrentClassification { get; }

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
