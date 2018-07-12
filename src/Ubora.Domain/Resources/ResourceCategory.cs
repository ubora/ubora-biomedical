using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources
{
    public class ResourceCategory : Entity<ResourceCategory>
    {
        public Guid Id { get; private set; }
        public Guid? ParentCategoryId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int MenuPriority { get; private set; }

        private void Apply(ResourceCategoryEditedEvent @event)
        {
            if (@event.ParentCategoryId == @event.CategoryId)
            {
                throw new InvalidOperationException($"There is a circular reference. ID: {@event.ParentCategoryId}");
            }

            Id = @event.CategoryId;
            ParentCategoryId = @event.ParentCategoryId;
            Title = @event.Title;
            Description = @event.Description;
            MenuPriority = @event.MenuPriority;
        }
    }
}