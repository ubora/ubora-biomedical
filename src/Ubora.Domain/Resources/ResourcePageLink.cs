using System;

namespace Ubora.Domain.Resources
{
    public class ResourcePageLink : IResourceMenuLink
    {
        public ResourcePageLink(Guid id, Guid? parentCategoryId, string title, int menuPriority)
        {
            Id = id;
            ParentCategoryId = parentCategoryId;
            Title = title;
            MenuPriority = menuPriority;
        }

        public Guid Id { get; }
        public Guid? ParentCategoryId { get; }
        public string Title { get; }
        public int MenuPriority { get; }

        public ResourcePageLink Edit(string title, int menuPriority, Guid? parentCategoryId)
        {
            return new ResourcePageLink(
                id: Id,
                title: title,
                menuPriority: menuPriority,
                parentCategoryId: parentCategoryId);
        }
    }
}