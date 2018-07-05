using System;

namespace Ubora.Domain.Resources
{
    // TODO: immutable
    public class ResourceCategoryLink : ILink
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int MenuPriority { get; set; }
        public string Description { get; set; }
        public Slug Slug { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}