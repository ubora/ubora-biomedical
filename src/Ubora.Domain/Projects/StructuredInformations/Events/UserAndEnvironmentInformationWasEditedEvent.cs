using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.StructuredInformations.Events
{
    public class UserAndEnvironmentInformationWasEditedEvent : ProjectEvent, IDeviceStructuredInformationEvent
    {
        public UserAndEnvironmentInformationWasEditedEvent(
            UserInfo initiatedBy, 
            Guid projectId, 
            Guid deviceStructuredInformationId, 
            DeviceStructuredInformationWorkpackageTypes workpackageType, 
            UserAndEnvironmentInformation userAndEnvironmentInformation) 
            : base(initiatedBy, projectId)
        {
            DeviceStructuredInformationId = (deviceStructuredInformationId == default(Guid)) ? projectId : deviceStructuredInformationId; // Warning: backwards-compatibility (ubora-kahawa)
            WorkpackageType = workpackageType;
            UserAndEnvironmentInformation = userAndEnvironmentInformation;
        }

        public Guid DeviceStructuredInformationId { get; }
        public DeviceStructuredInformationWorkpackageTypes WorkpackageType { get; }
        public UserAndEnvironmentInformation UserAndEnvironmentInformation { get; }
        
        public override string GetDescription() => "edited device structured information.";
    }
}