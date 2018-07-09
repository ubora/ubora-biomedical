using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories.Models
{
    public class CreateResourceCategoryPostModel
    {
        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public Guid? ParentCategoryId { get; set; }

        public int MenuPriority { get; set; }
    }
}
