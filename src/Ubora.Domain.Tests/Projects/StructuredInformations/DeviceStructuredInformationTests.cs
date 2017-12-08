using System;
using FluentAssertions;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.StructuredInformations
{
    public class DeviceStructuredInformationTests
    {
        [Fact]
        public void UserAndEnvironmentInformation_Can_Not_Be_Edited_When_Workpackage_One_Has_Not_Been_Accepted()
        {
            var deviceStructuredInformation = new DeviceStructuredInformation();

            var @event = new UserAndEnvironmentInformationWasEditedEvent(
                initiatedBy: new DummyUserInfo(), 
                projectId: Guid.NewGuid(),
                userAndEnvironmentInformation: UserAndEnvironmentInformation.CreateEmpty());

            // Act
            Action act = () => deviceStructuredInformation.Apply(@event);

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void HealthTechnologySpecificationInformation_Can_Not_Be_Edited_When_Workpackage_One_Has_Not_Been_Accepted()
        {
            var deviceStructuredInformation = new DeviceStructuredInformation();

            var @event = new HealthTechnologySpecificationInformationWasEditedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: Guid.NewGuid(),
                healthTechnologySpecificationsInformation: new HealthTechnologySpecificationsInformation());

            // Act
            Action act = () => deviceStructuredInformation.Apply(@event);

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
