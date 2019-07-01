using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.Projects.BusinessModelCanvases;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class OpenWorkpackageFiveCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void WP5_Can_Be_Unlocked()
        {
            var project = 
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp3Unlocked()
                    .WithWp4Unlocked()
                    .Seed(this);

            // Act
            var result = Processor.Execute(new OpenWorkpackageFiveCommand
            {
                ProjectId = project.Id,
                Actor = new DummyUserInfo()
            });

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Assert WP4
            var wp5 = Session.Load<WorkpackageFive>(project.Id);

            using (new AssertionScope())
            {
                wp5.Should().NotBeNull();
                wp5.ProjectId.Should().Be(project.Id);
                wp5.Title.Should().Be("Operation");
                wp5.Steps.Should().HaveCount(1);
            }

            // Assert business model canvas
            var businessModelCanvas = 
                Session
                    .Query<BusinessModelCanvas>()
                    .Where(x => x.ProjectId == project.Id)
                    .SingleOrDefault();
            using (new AssertionScope())
            {
                businessModelCanvas.Should().NotBeNull();
                businessModelCanvas.KeyResourcesAndPartnersDescription.Should().Be(new QuillDelta());
                businessModelCanvas.PotentialClientsAndUsersAndChannelsDescription.Should().Be(new QuillDelta());
                businessModelCanvas.RelevantDocumentationForProductionAndUseDescription.Should().Be(new QuillDelta());
                businessModelCanvas.ValueProposalDescription.Should().Be(new QuillDelta());
                businessModelCanvas.GrowthStrategyDescription.Should().Be(new QuillDelta());
                businessModelCanvas.AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription.Should().Be(new QuillDelta());
            }
        }
    }
}
