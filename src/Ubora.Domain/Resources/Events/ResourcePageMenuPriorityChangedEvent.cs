using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageMenuPriorityChangedEvent : UboraEvent
    {
        public ResourcePageMenuPriorityChangedEvent(UserInfo initiatedBy, Guid resourcePageId, int menuPriority) : base(initiatedBy)
        {
            ResourcePageId = resourcePageId;
            MenuPriority = menuPriority;
        }

        public Guid ResourcePageId { get; }
        public int MenuPriority { get; }

        public override string GetDescription() => "changed menu priority.";
    }
}