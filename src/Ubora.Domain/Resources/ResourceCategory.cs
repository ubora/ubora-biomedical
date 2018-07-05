using System;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources
{
    public class ResourceCategory
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MenuPriority { get; set; }
        public Slug ActiveSlug { get; private set; }

        private void Apply(ResourceCategoryEditedEvent @event)
        {
            Id = @event.CategoryId;
            ParentCategoryId = @event.ParentCategoryId;
            Title = @event.Title;
            Description = @event.Description;
            MenuPriority = @event.MenuPriority;
            ActiveSlug = @event.Slug;
        }
    }
}