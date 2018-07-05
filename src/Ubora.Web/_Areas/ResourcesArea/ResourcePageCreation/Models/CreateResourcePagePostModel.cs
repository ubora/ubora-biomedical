using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePageCreation.Models
{
    public class CreateResourcePagePostModel
    {
        [Required(ErrorMessage = "Please specify a title.")]
        [StringLength(100)]
        public string Title { get; set; }

        public string Body { get; set; } // TODO required validation

        public int MenuPriority { get; set; }

        public Guid ParentCategoryId { get; set; }
    }
}