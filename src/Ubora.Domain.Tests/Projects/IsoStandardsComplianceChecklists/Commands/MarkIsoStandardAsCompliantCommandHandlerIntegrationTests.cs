using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.IsoStandardsComplianceChecklists.Commands
{
    public class MarkIsoStandardAsCompliantCommandHandlerIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Standard_Can_Be_Marked_As_Compliant()
        {
            var project =
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp4Unlocked()
                    .Seed(this);

            var aggregate = Session.Load<IsoStandardsComplianceChecklist>(project.Id);
            var standardToMarkAsCompliant = aggregate.IsoStandards.Skip(1).First();

            var command = new MarkIsoStandardAsCompliantCommand
            {
                IsoStandardId = standardToMarkAsCompliant.Id,
                ProjectId = project.Id,
                Actor = new DummyUserInfo()
            };

            // Act
            Processor.Execute(command);

            // Assert
            aggregate = Session.Load<IsoStandardsComplianceChecklist>(project.Id);
            var standardAfterMarking = aggregate.IsoStandards.Single(x => x.Id == standardToMarkAsCompliant.Id);

            using (new AssertionScope())
            {
                standardAfterMarking.ShouldBeEquivalentTo(standardToMarkAsCompliant, opt => opt.Excluding(x => x.IsMarkedAsCompliant));
                standardAfterMarking.IsMarkedAsCompliant.Should().BeTrue();
            }
        }
    }
}