using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources
{
    public class ResourcePage : Entity<ResourcePage>
    {
        public Guid Id { get; private set; }
        public Slug ActiveSlug { get; private set; }

        public Guid ContentVersion { get; private set; }
        public ResourceContent Content { get; private set; }
        
        public bool IsDeleted { get; private set; }
        public int MenuOrder { get; set; } // TODO: Option to set the order of the resource in the side menu. Default sort by first letter.
        public string Category { get; set; } // TODO!

        public string GetBlobContainerName() => $"ResourcePage_{Id}";

        private void SetContent(ResourceContent content)
        {
            Content = content;
            ContentVersion = Guid.NewGuid();
        }
        
        private void Apply(ResourceCreatedEvent @event)
        {
            if (@event.ResourceId == default(Guid))
                throw new ArgumentException(nameof(@event.ResourceId));
            
            Id = @event.ResourceId;
            ActiveSlug = @event.Slug;
            SetContent(@event.Content);

            Category = Guid.NewGuid().ToString();
        }

        private void Apply(ResourceContentEditedEvent @event)
        {
            if (IsDeleted)
                throw new InvalidOperationException();

            if (ContentVersion != @event.PreviousContentVersion)
                throw new InvalidOperationException("Content has been changed -- the versions don't match.");
            
            SetContent(@event.Content);
        }
        
        private void Apply(ResourcePageDeletedEvent @event)
        {
            IsDeleted = true;
        }
    }
}