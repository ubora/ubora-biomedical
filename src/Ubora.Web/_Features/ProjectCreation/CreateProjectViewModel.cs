using System.ComponentModel.DataAnnotations;
using Ubora.Web._Features.Projects.Workpackages.Steps;

namespace Ubora.Web._Features.ProjectCreation
{
    public class CreateProjectViewModel : DesignPlanningViewModel
    {
        [Required]
        [Display(Name = "Project title")]
        public string Title { get; set; }
    }
}