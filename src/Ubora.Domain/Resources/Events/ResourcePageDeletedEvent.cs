using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageDeletedEvent : UboraEvent
    {
        public Guid ResourcePageId { get; }

        public ResourcePageDeletedEvent(UserInfo initiatedBy, Guid resourcePageId) : base(initiatedBy)
        {
            ResourcePageId = resourcePageId;
        }

        public override string GetDescription() => "deleted resource page.";
    }
}
