using System;

namespace Ubora.Domain.Resources
{
    public class ResourceCategoryLink : IResourceMenuLink
    {
        public ResourceCategoryLink(Guid id, string title, int menuPriority, string description, Guid? parentCategoryId)
        {
            Id = id;
            Title = title;
            MenuPriority = menuPriority;
            Description = description;
            ParentCategoryId = parentCategoryId;
        }

        public Guid Id { get; }
        public string Title { get; }
        public int MenuPriority { get; }
        public string Description { get; }
        public Guid? ParentCategoryId { get; }

        public ResourceCategoryLink Edit(string title, int menuPriority, string description, Guid? parentCategoryId)
        {
            return new ResourceCategoryLink(
                id: Id,
                title: title,
                menuPriority: menuPriority,
                description: description,
                parentCategoryId: parentCategoryId);
        }
    }
}