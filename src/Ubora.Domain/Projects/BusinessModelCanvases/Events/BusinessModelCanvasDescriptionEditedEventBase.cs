using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.BusinessModelCanvases.Events
{
    public abstract class BusinessModelCanvasDescriptionEditedEventBase : ProjectEvent
    {
        public BusinessModelCanvasDescriptionEditedEventBase(UserInfo initiatedBy, Guid projectId, Guid aggregateId, QuillDelta value)
            : base(initiatedBy, projectId)
        {
            AggregateId = aggregateId;
            Value = value;
        }

        public Guid AggregateId { get; }
        public QuillDelta Value { get; }
    }
}
