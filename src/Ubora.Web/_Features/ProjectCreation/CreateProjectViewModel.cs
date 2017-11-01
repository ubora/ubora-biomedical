using System.ComponentModel.DataAnnotations;
using Ubora.Web._Features.Projects.Workpackages.Steps;

namespace Ubora.Web._Features.ProjectCreation
{
    public class CreateProjectViewModel : ProjectOverviewViewModel
    {
        [Required]
        [Display(Name = "Project title")]
        [StringLength(100, ErrorMessage = "The {0} can be max {1} characters long.")]
        public string Title { get; set; }
    }
}