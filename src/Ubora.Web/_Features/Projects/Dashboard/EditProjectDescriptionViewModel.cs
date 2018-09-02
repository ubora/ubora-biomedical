using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class EditProjectTitleAndDescriptionViewModel
    {
        [Required]
        [Display(Name = "Project title")]
        [StringLength(100, ErrorMessage = "The {0} can be max {1} characters long.")]
        public string Title { get; set; }
        public string DescriptionQuillDelta { get; set; }
    }
}
