using System;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories.Models
{
    public class EditResourceCategoryPostModel
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int MenuPriority { get; set; }
    }
}
