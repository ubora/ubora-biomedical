using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.StructuredInformations.Events;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class DeviceStructuredInformation : Entity<DeviceStructuredInformation>, IProjectEntity
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public UserAndEnvironmentInformation UserAndEnvironment { get; private set; }
        public HealthTechnologySpecificationsInformation HealthTechnologySpecification { get; private set; }

        private void Apply(WorkpackageOneReviewAcceptedEvent e)
        {
            Id = e.ProjectId;
            ProjectId = e.ProjectId;

            UserAndEnvironment = UserAndEnvironmentInformation.CreateEmpty();
            HealthTechnologySpecification = HealthTechnologySpecificationsInformation.CreateEmpty();
        }

        private void Apply(UserAndEnvironmentInformationWasEditedEvent e)
        {
            if (Id == default(Guid)) throw new InvalidOperationException();
            if (ProjectId == default(Guid)) throw new InvalidOperationException();

            UserAndEnvironment = e.UserAndEnvironmentInformation ?? throw new InvalidOperationException();
        }

        private void Apply(HealthTechnologySpecificationInformationWasEditedEvent e)
        {
            if (Id == default(Guid)) throw new InvalidOperationException();
            if (ProjectId == default(Guid)) throw new InvalidOperationException();
        }
    }
}
