using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.IsoStandardsComplianceChecklists.Commands
{
    public class AddIsoStandardCommandHandlerIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Standard_Can_Be_Added_To_Checklist()
        {
            var project =
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp4Unlocked()
                    .Seed(this);

            var command = AutoFixture.Create<AddIsoStandardCommand>();
            command.ProjectId = project.Id;

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var aggregate = Session.Load<IsoStandardsComplianceChecklist>(project.Id);
            var standard = aggregate.IsoStandards.Last();

            using (new AssertionScope())
            {
                standard.Id.Should().NotBe(Guid.Empty);
                standard.Title.Should().Be(command.Title);
                standard.Link.Should().Be(command.Link);
                standard.ShortDescription.Should().Be(command.ShortDescription);
                standard.IsMarkedAsCompliant.Should().BeFalse();
                standard.AddedByUserId.Should().Be(command.Actor.UserId);
            }
        }
    }
}