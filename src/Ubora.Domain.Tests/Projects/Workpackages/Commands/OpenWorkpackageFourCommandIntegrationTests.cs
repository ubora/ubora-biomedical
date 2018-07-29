using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Specifications;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class OpenWorkpackageFourCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void WP4_Can_Be_Unlocked()
        {
            var project = 
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp3Unlocked()
                    .Seed(this);

            // Act
            var result = Processor.Execute(new OpenWorkpackageFourCommand
            {
                ProjectId = project.Id,
                Actor = new DummyUserInfo()
            });

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Assert WP4
            var wp4 = Session.Load<WorkpackageFour>(project.Id);

            using (new AssertionScope())
            {
                wp4.Should().NotBeNull();
                wp4.ProjectId.Should().Be(project.Id);
                wp4.Title.Should().Be("Implementation");
                wp4.Steps.Should().HaveCount(3);
            }

            // Assert device structured information
            var deviceStructuredInformation = 
                Session
                    .Query<DeviceStructuredInformation>()
                    .Where(x => x.ProjectId == project.Id)
                    .Where(new DeviceStructuredInformationFromWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes.Four))
                    .SingleOrDefault();

            using (new AssertionScope())
            {
                deviceStructuredInformation.Should().NotBeNull();
                deviceStructuredInformation.Id.Should().NotBe(project.Id);
            }

            // Assert ISO compliance checklist
            var isoStandardsComplianceChecklist = Session.Load<IsoStandardsComplianceChecklist>(project.Id);

            using (new AssertionScope())
            {
                isoStandardsComplianceChecklist.Should().NotBeNull();
                isoStandardsComplianceChecklist.ProjectId.Should().Be(project.Id);
                isoStandardsComplianceChecklist.IsoStandards.Should().HaveCount(5);
            }
        }
    }
}
