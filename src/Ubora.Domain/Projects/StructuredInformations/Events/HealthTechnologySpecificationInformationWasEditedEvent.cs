using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.StructuredInformations.Events
{
    public class HealthTechnologySpecificationInformationWasEditedEvent : ProjectEvent, IDeviceStructuredInformationEvent
    {
        public HealthTechnologySpecificationInformationWasEditedEvent(
            UserInfo initiatedBy,
            Guid projectId,
            Guid deviceStructuredInformationId,
            DeviceStructuredInformationWorkpackageTypes workpackageType,
            HealthTechnologySpecificationsInformation healthTechnologySpecificationsInformation) 
            : base(initiatedBy, projectId)
        {
            DeviceStructuredInformationId = (deviceStructuredInformationId == default(Guid)) ? projectId : deviceStructuredInformationId; // Warning: backwards-compatibility (ubora-kahawa)
            WorkpackageType = workpackageType;
            HealthTechnologySpecificationsInformation = healthTechnologySpecificationsInformation;
        }

        public Guid DeviceStructuredInformationId { get; }
        public DeviceStructuredInformationWorkpackageTypes WorkpackageType { get; }
        public HealthTechnologySpecificationsInformation HealthTechnologySpecificationsInformation { get; }

        public override string GetDescription() => "edited device structured information.";
    }
}
