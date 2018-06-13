using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageDeletedEvent : UboraEvent
    {
        public ResourcePageDeletedEvent(UserInfo initiatedBy, Guid resourceId) : base(initiatedBy)
        {
            ResourceId = resourceId;
        }
        
        public Guid ResourceId { get; }

        public override string GetDescription() => "Deleted resource.";
    }
}