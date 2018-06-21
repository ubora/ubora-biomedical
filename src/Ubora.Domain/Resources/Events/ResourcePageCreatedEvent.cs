using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageCreatedEvent : UboraEvent
    {
        public Guid ResourceId { get; }
        public Slug Slug { get; }
        public ResourceContent Content { get; }
        public int MenuPriority { get; }

        public ResourcePageCreatedEvent(UserInfo initiatedBy, Guid resourceId, Slug slug, ResourceContent content, int menuPriority) 
            : base(initiatedBy)
        {
            ResourceId = resourceId;
            Slug = slug;
            Content = content;
            MenuPriority = menuPriority;
        }

        public override string GetDescription() => "Created resource.";
    }
}