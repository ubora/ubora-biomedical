using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ubora.Domain;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class ProjectOverviewViewModel : ITagsAndKeywords
    {
        [Required]
        [Display(Name = "Clinical need")]
        public string ClinicalNeedTag { get; set; }

        [Required]
        [Display(Name = "Area")]
        public string AreaOfUsageTag { get; set; }

        [Required]
        [Display(Name = "Technology")]
        public string PotentialTechnologyTag { get; set; }

        [Display(Name = "Project keywords")]
        public string Keywords { get; set; }
    }
}
