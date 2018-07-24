using System;

namespace Ubora.Domain.Resources
{
    public interface IResourceMenuLink
    {
        Guid Id { get; }
        Guid? ParentCategoryId { get; }
        string Title { get; }
        int MenuPriority { get; }
    }
}