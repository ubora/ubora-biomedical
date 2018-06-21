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
        
        public int MenuPriority { get; set; }

        public string GetBlobContainerName() => $"ResourcePage_{Id}";

        private void SetContent(ResourceContent content)
        {
            Content = content;
            ContentVersion = Guid.NewGuid();
        }
        
        private void Apply(ResourcePageCreatedEvent @event)
        {
            if (@event.ResourceId == default(Guid))
                throw new ArgumentException(nameof(@event.ResourceId));
            
            Id = @event.ResourceId;
            ActiveSlug = @event.Slug;
            SetContent(@event.Content);
            MenuPriority = @event.MenuPriority;
        }

        private void Apply(ResourcePageContentEditedEvent @event)
        {
            if (ContentVersion != @event.PreviousContentVersion)
                throw new InvalidOperationException("Content has been changed -- the versions don't match.");
            
            SetContent(@event.Content);
        }
    }
}