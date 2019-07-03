using FluentAssertions;
using System.Linq;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.BusinessModelCanvases;
using Ubora.Domain.Projects.BusinessModelCanvases.Command;
using Ubora.Domain.Projects.BusinessModelCanvases.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.BusinessModelCanvases.Commands
{
    public class EditBusinessModelCanvasCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Edits_Business_Model_Canvas_Descriptions_When_Changed()
        {
            var project =
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp3Unlocked()
                    .WithWp4Unlocked()
                    .WithWp5Unlocked()
                    .Seed(this);

            var valuepProposalDescription = new QuillDelta("{ValueProposalDescription}");
            var growthStrategyDescription = new QuillDelta("{GrowthStrategyDescription}");
            var keyResourcesAndPartnersDescription = new QuillDelta("{KeyResourcesAndPartnersDescription}");
            var potentialClientsAndUsersAndChannelsDescription = new QuillDelta("{PotentialClientsAndUsersAndChannelsDescription}");
            var relevantDocumentationForProductionAndUseDescription = new QuillDelta("{RelevantDocumentationForProductionAndUseDescription}");
            var analysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription = new QuillDelta("{AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription}");

            var actor = new DummyUserInfo();
            var command = new EditBusinessModelCanvasCommand
            {
                Actor = actor,
                ProjectId = project.Id,
                ValueProposalDescription = valuepProposalDescription,
                GrowthStrategyDescription = growthStrategyDescription,
                KeyResourcesAndPartnersDescription = keyResourcesAndPartnersDescription,
                PotentialClientsAndUsersAndChannelsDescription = potentialClientsAndUsersAndChannelsDescription,
                RelevantDocumentationForProductionAndUseDescription = relevantDocumentationForProductionAndUseDescription,
                AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription = analysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription
            };

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var events = Session.Events.FetchStream(project.Id).Select(x => x.Data).TakeLast(6);

            events.Should().Contain(x => x is ValueProposalDescriptionEditedEvent);
            events.Should().Contain(x => x is GrowthStrategyDescriptionEditedEvent);
            events.Should().Contain(x => x is KeyResourcesAndPartnersDescriptionEditedEvent);
            events.Should().Contain(x => x is PotentialClientsAndUsersAndChannelsDescriptionEditedEvent);
            events.Should().Contain(x => x is RelevantDocumentationForProductionAndUseDescriptionEditedEvent);
            events.Should().Contain(x => x is AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescriptionEditedEvent);
            events.Should().OnlyContain(x => ((UboraEvent)x).InitiatedBy == actor);

            var businessModelCanvas = Session.Load<BusinessModelCanvas>(project.Id);

            businessModelCanvas.ValueProposalDescription.Should().Be(valuepProposalDescription);
            businessModelCanvas.GrowthStrategyDescription.Should().Be(growthStrategyDescription);
            businessModelCanvas.KeyResourcesAndPartnersDescription.Should().Be(keyResourcesAndPartnersDescription);
            businessModelCanvas.PotentialClientsAndUsersAndChannelsDescription.Should().Be(potentialClientsAndUsersAndChannelsDescription);
            businessModelCanvas.RelevantDocumentationForProductionAndUseDescription.Should().Be(relevantDocumentationForProductionAndUseDescription);
            businessModelCanvas.AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription.Should().Be(analysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription);
        }

        [Fact]
        public void Does_Not_Persists_Events_When_Not_Changed() 
        {
            var project =
                new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp3Unlocked()
                    .WithWp4Unlocked()
                    .WithWp5Unlocked()
                    .Seed(this);
            var businessModelCanvas = Session.Load<BusinessModelCanvas>(project.Id);
            var command = new EditBusinessModelCanvasCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = project.Id,
                ValueProposalDescription = businessModelCanvas.ValueProposalDescription,
                GrowthStrategyDescription = businessModelCanvas.GrowthStrategyDescription,
                KeyResourcesAndPartnersDescription = businessModelCanvas.KeyResourcesAndPartnersDescription,
                PotentialClientsAndUsersAndChannelsDescription = businessModelCanvas.PotentialClientsAndUsersAndChannelsDescription,
                RelevantDocumentationForProductionAndUseDescription = businessModelCanvas.RelevantDocumentationForProductionAndUseDescription,
                AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription = businessModelCanvas.AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription
            };

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var events = Session.Events.FetchStream(project.Id).Select(x => x.Data);

            events.Should().NotContain(x => x is ValueProposalDescriptionEditedEvent);
            events.Should().NotContain(x => x is GrowthStrategyDescriptionEditedEvent);
            events.Should().NotContain(x => x is KeyResourcesAndPartnersDescriptionEditedEvent);
            events.Should().NotContain(x => x is PotentialClientsAndUsersAndChannelsDescriptionEditedEvent);
            events.Should().NotContain(x => x is RelevantDocumentationForProductionAndUseDescriptionEditedEvent);
            events.Should().NotContain(x => x is AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescriptionEditedEvent);
        }
    }
}
