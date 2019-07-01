using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.Projects.BusinessModelCanvases;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class OpenWorkpackageSixCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void WP6_Can_Be_Unlocked()
        {
            var project = 
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .Seed(this);

            Session.Load<WorkpackageSix>(project.Id).Should().BeNull();

            // Act
            var result = Processor.Execute(new OpenWorkpackageSixCommand
            {
                ProjectId = project.Id,
                Actor = new DummyUserInfo()
            });

            // Assert
            result.IsSuccess.Should().BeTrue();

            var wp6 = Session.Load<WorkpackageSix>(project.Id);

            using (new AssertionScope())
            {
                wp6.Should().NotBeNull();
                wp6.ProjectId.Should().Be(project.Id);
                wp6.Title.Should().Be("Project closure");
                wp6.Steps.Should().HaveCount(3);
            }
        }
    }
}
