using System;

namespace Ubora.Domain.Resources
{
    public interface ILink
    {
        Guid Id { get; }
        Guid? ParentCategoryId { get; }
        string Title { get; }
        int MenuPriority { get; }
    }
}