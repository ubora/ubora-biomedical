using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.IsoStandardsComplianceChecklists.Commands
{
    public class RemoveIsoStandardCommandHandlerIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Standard_Can_Be_Removed_From_Checklist()
        {
            var project =
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp4Unlocked()
                    .Seed(this);

            var aggregate = Session.Load<IsoStandardsComplianceChecklist>(project.Id);
            var initialStandardCount = aggregate.IsoStandards.Count();
            var standardToRemove = aggregate.IsoStandards.Skip(1).First();

            var command = new RemoveIsoStandardCommand
            {
                IsoStandardId = standardToRemove.Id,
                ProjectId = project.Id,
                Actor = new DummyUserInfo()
            };

            // Act
            Processor.Execute(command);

            // Assert
            aggregate = Session.Load<IsoStandardsComplianceChecklist>(project.Id);

            using (new AssertionScope())
            {
                aggregate.IsoStandards.Count.Should().Be(initialStandardCount - 1);
                aggregate.IsoStandards.Should().NotContain(x => x.Id == standardToRemove.Id);
            }
        }
    }
}