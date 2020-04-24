using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.BusinessModelCanvases.Events;

namespace Ubora.Domain.Projects.BusinessModelCanvases.Command
{
    public class EditBusinessModelCanvasCommand : UserProjectCommand
    {
        public QuillDelta ValueProposalDescription { get; set; }
        public QuillDelta GrowthStrategyDescription { get; set; }
        public QuillDelta KeyResourcesAndPartnersDescription { get; set; }
        public QuillDelta PotentialClientsAndUsersAndChannelsDescription { get; set; }
        public QuillDelta RelevantDocumentationForProductionAndUseDescription { get; set; }
        public QuillDelta AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription { get; set; }

        public class Handler : ICommandHandler<EditBusinessModelCanvasCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(EditBusinessModelCanvasCommand cmd)
            {
                var businessModelCanvas = _documentSession.LoadOrThrow<BusinessModelCanvas>(cmd.ProjectId);

                var events = new List<object>();

                if (businessModelCanvas.ValueProposalDescription != cmd.ValueProposalDescription) 
                    events.Add(new ValueProposalDescriptionEditedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.ValueProposalDescription));

                if (businessModelCanvas.GrowthStrategyDescription != cmd.GrowthStrategyDescription) 
                    events.Add(new GrowthStrategyDescriptionEditedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.GrowthStrategyDescription));

                if (businessModelCanvas.KeyResourcesAndPartnersDescription != cmd.KeyResourcesAndPartnersDescription) 
                    events.Add(new KeyResourcesAndPartnersDescriptionEditedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.KeyResourcesAndPartnersDescription));

                if (businessModelCanvas.PotentialClientsAndUsersAndChannelsDescription != cmd.PotentialClientsAndUsersAndChannelsDescription) 
                    events.Add(new PotentialClientsAndUsersAndChannelsDescriptionEditedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.PotentialClientsAndUsersAndChannelsDescription));

                if (businessModelCanvas.RelevantDocumentationForProductionAndUseDescription != cmd.RelevantDocumentationForProductionAndUseDescription) 
                    events.Add(new RelevantDocumentationForProductionAndUseDescriptionEditedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.RelevantDocumentationForProductionAndUseDescription));

                if (businessModelCanvas.AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription != cmd.AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription) 
                    events.Add(new AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescriptionEditedEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription));

                if (events.Any()) 
                {
                    _documentSession.Events.Append(cmd.ProjectId, events.ToArray());
                    _documentSession.SaveChanges();
                }

                return CommandResult.Success;
            }
        }
    }
}