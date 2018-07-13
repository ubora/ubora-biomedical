using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.StructuredInformations.Events
{
    public class HealthTechnologySpecificationInformationWasEditedEvent : ProjectEvent
    {
        public HealthTechnologySpecificationInformationWasEditedEvent(Guid deviceStructuredInformationId, WorkpackageType workpackageType, UserInfo initiatedBy, Guid projectId, HealthTechnologySpecificationsInformation healthTechnologySpecificationsInformation) : base(initiatedBy, projectId)
        {
            DeviceStructuredInformationId = deviceStructuredInformationId;
            WorkpackageType = workpackageType;
            HealthTechnologySpecificationsInformation = healthTechnologySpecificationsInformation;
        }

        public Guid DeviceStructuredInformationId { get; private set; }
        public WorkpackageType WorkpackageType { get; private set; }
        public HealthTechnologySpecificationsInformation HealthTechnologySpecificationsInformation { get; private set; }

        public override string GetDescription() => "edited device structured information.";
    }
}
