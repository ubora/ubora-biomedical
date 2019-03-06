using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.BusinessModelCanvases.Events
{
    public class GrowthStrategyDescriptionEditedEvent : BusinessModelCanvasDescriptionEditedEventBase
    {
        public GrowthStrategyDescriptionEditedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, QuillDelta value) 
            : base(initiatedBy, projectId, aggregateId, value)
        {
        }

        public override string GetDescription() => "edited on business model canvas the description of growth strategy.";
    }
}