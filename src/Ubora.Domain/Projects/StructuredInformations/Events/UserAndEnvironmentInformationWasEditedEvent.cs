using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.StructuredInformations.Events
{
    public class UserAndEnvironmentInformationWasEditedEvent : ProjectEvent
    {
        public UserAndEnvironmentInformationWasEditedEvent(DeviceStructuredInformationWorkpackageTypes workpackageType, UserInfo initiatedBy, Guid projectId, UserAndEnvironmentInformation userAndEnvironmentInformation) : base(initiatedBy, projectId)
        {
            WorkpackageType = workpackageType;
            UserAndEnvironmentInformation = userAndEnvironmentInformation;
        }
        
        public DeviceStructuredInformationWorkpackageTypes WorkpackageType { get; private set; }
        public UserAndEnvironmentInformation UserAndEnvironmentInformation { get; private set; }
        
        public override string GetDescription() => "edited device structured information.";
    }
}