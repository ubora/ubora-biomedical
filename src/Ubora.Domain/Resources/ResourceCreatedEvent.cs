using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources
{
    public class ResourceCreatedEvent : UboraEvent
    {
        public Guid ResourceId { get; }
        public Slug Slug { get; }
        public ResourceContent Content { get; }

        public ResourceCreatedEvent(UserInfo initiatedBy, Guid resourceId, Slug slug, ResourceContent content) 
            : base(initiatedBy)
        {
            ResourceId = resourceId;
            Slug = slug;
            Content = content;
        }

        public override string GetDescription() => "Created resource.";
    }
}