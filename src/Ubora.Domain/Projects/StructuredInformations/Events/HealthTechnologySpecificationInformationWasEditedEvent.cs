﻿using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.StructuredInformations.Events
{
    public class HealthTechnologySpecificationInformationWasEditedEvent : ProjectEvent
    {
        public HealthTechnologySpecificationInformationWasEditedEvent(DeviceStructuredInformationWorkpackageTypes workpackageType, UserInfo initiatedBy, Guid projectId, HealthTechnologySpecificationsInformation healthTechnologySpecificationsInformation) : base(initiatedBy, projectId)
        {
            WorkpackageType = workpackageType;
            HealthTechnologySpecificationsInformation = healthTechnologySpecificationsInformation;
        }

        public DeviceStructuredInformationWorkpackageTypes WorkpackageType { get; private set; }
        public HealthTechnologySpecificationsInformation HealthTechnologySpecificationsInformation { get; private set; }

        public override string GetDescription() => "edited device structured information.";
    }
}
