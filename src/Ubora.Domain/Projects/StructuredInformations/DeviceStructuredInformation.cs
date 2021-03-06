﻿using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.StructuredInformations.Events;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class DeviceStructuredInformation : Entity<DeviceStructuredInformation>, IProjectEntity
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public DeviceStructuredInformationWorkpackageTypes WorkpackageType { get; private set; }

        public UserAndEnvironmentInformation UserAndEnvironment { get; private set; }
        public HealthTechnologySpecificationsInformation HealthTechnologySpecification { get; private set; }
        public bool IsUserAndEnvironmentEdited { get; private set; }
        public bool IsHealthTechnologySpecificationEdited { get; private set; }

        private void Apply(WorkpackageOneReviewAcceptedEvent e)
        {
            if (WorkpackageType != DeviceStructuredInformationWorkpackageTypes.Two)
                throw new InvalidOperationException();

            Id = e.ProjectId;
            ProjectId = e.ProjectId;
            WorkpackageType = DeviceStructuredInformationWorkpackageTypes.Two;

            if (!IsUserAndEnvironmentEdited)
            {
                UserAndEnvironment = UserAndEnvironmentInformation.CreateEmpty();
            }

            if (!IsHealthTechnologySpecificationEdited)
            {
                HealthTechnologySpecification = HealthTechnologySpecificationsInformation.CreateEmpty();
            }
        }

        private void Apply(WorkpackageFourOpenedEvent e)
        {
            if (IsUserAndEnvironmentEdited || IsHealthTechnologySpecificationEdited)
                throw new InvalidOperationException();

            Id = e.DeviceStructuredInformationId;
            ProjectId = e.ProjectId;
            WorkpackageType = DeviceStructuredInformationWorkpackageTypes.Four;

            UserAndEnvironment = UserAndEnvironmentInformation.CreateEmpty();
            HealthTechnologySpecification = HealthTechnologySpecificationsInformation.CreateEmpty();
        }

        private void Apply(UserAndEnvironmentInformationWasEditedEvent e)
        {
            if (Id == default(Guid)) throw new InvalidOperationException();
            if (ProjectId == default(Guid)) throw new InvalidOperationException();

            WorkpackageType = e.WorkpackageType;
            UserAndEnvironment = e.UserAndEnvironmentInformation ?? throw new InvalidOperationException();
            IsUserAndEnvironmentEdited = true;
        }

        private void Apply(HealthTechnologySpecificationInformationWasEditedEvent e)
        {
            if (Id == default(Guid)) throw new InvalidOperationException();
            if (ProjectId == default(Guid)) throw new InvalidOperationException();

            WorkpackageType = e.WorkpackageType;
            HealthTechnologySpecification =
                e.HealthTechnologySpecificationsInformation ?? throw new InvalidOperationException();
            IsHealthTechnologySpecificationEdited = true;
        }
    }
}