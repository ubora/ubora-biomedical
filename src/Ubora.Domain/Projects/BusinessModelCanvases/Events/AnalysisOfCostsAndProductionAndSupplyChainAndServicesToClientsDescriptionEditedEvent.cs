using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.BusinessModelCanvases.Events
{
    public class AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescriptionEditedEvent
        : BusinessModelCanvasDescriptionEditedEventBase
    {
        public AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescriptionEditedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, QuillDelta value) 
            : base(initiatedBy, projectId, aggregateId, value)
        {
        }

        public override string GetDescription() => "edited on business model canvas the description of analysis of costs, production, supply chain and services to clients.";
    }
}