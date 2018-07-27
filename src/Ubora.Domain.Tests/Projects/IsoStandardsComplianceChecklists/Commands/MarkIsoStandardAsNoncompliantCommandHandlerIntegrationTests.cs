using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.IsoStandardsComplianceChecklists.Commands
{
    public class MarkIsoStandardAsNoncompliantCommandHandlerIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Standard_Can_Be_Marked_As_Noncompliant()
        {
            var project =
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp4Unlocked()
                    .Seed(this);

            var aggregate = Session.Load<IsoStandardsComplianceChecklist>(project.Id);
            var standardToMarkAsNoncompliant = aggregate.IsoStandards.Skip(1).First();

            Processor.Execute(new MarkIsoStandardAsCompliantCommand
            {
                IsoStandardId = standardToMarkAsNoncompliant.Id,
                ProjectId = project.Id,
                Actor = new DummyUserInfo()
            });

            aggregate = Session.Load<IsoStandardsComplianceChecklist>(project.Id);
            aggregate.IsoStandards.Single(x => x.Id == standardToMarkAsNoncompliant.Id)
                .IsMarkedAsCompliant.Should().BeTrue();

            var command = new MarkIsoStandardAsNoncompliantCommand
            {
                IsoStandardId = standardToMarkAsNoncompliant.Id,
                ProjectId = project.Id,
                Actor = new DummyUserInfo()
            };

            // Act
            Processor.Execute(command);

            // Assert
            aggregate = Session.Load<IsoStandardsComplianceChecklist>(project.Id);
            var standardAfterMarking = aggregate.IsoStandards.Single(x => x.Id == standardToMarkAsNoncompliant.Id);

            using (new AssertionScope())
            {
                standardAfterMarking.ShouldBeEquivalentTo(standardToMarkAsNoncompliant, opt => opt.Excluding(x => x.IsMarkedAsCompliant));
                standardAfterMarking.IsMarkedAsCompliant.Should().BeFalse();
            }
        }
    }
}