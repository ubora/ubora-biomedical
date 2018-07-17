using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageTitleChangedEvent : UboraEvent
    {
        public ResourcePageTitleChangedEvent(UserInfo initiatedBy, Guid resourcePageId, string title) : base(initiatedBy)
        {
            ResourcePageId = resourcePageId;
            Title = title;
        }

        public Guid ResourcePageId { get; }
        public string Title { get; }

        public override string GetDescription() => "changed resource page title.";
    }
}
