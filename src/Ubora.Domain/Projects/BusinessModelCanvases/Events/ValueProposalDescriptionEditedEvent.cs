using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.BusinessModelCanvases.Events
{
    public class ValueProposalDescriptionEditedEvent : ProjectEvent
    {
        public ValueProposalDescriptionEditedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, QuillDelta value) 
            : base(initiatedBy, projectId)
        {
            AggregateId = aggregateId;
            Value = value;
        }

        public Guid AggregateId { get; }
        public QuillDelta Value { get; }

        public override string GetDescription() => "edited on business model canvas the description of value proposal.";
    }
}
