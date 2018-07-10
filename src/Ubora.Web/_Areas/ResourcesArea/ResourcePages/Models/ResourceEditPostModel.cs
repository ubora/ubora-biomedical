using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class ResourceEditPostModel
    {
        public Guid ResourceId { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int MenuPriority { get; set; }
        public string Body { get; set; }
        public int ContentVersion { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
    }
}