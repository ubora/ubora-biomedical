using System;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Resources
{
    public class Resource : Entity<Resource>
    {
        public Guid Id { get; private set; }
        public Slug Slug { get; private set; }

        public Guid ContentVersion { get; private set; }
        public ResourceContent Content { get; private set; }
        
        public bool IsDeleted { get; private set; }
        public int Order { get; set; } // TODO: Option to set the order of the resource in the side menu. Default sort by first letter.
        public string Category { get; set; } // TODO!

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
            Slug = @event.Slug;
            SetContent(@event.Content);
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