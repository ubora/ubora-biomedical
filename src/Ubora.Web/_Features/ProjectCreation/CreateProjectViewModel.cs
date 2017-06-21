using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.ProjectCreation
{
    public class CreateProjectViewModel
    {
        [Required]
        [Display(Name = "Project title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Clinical need")]
        public string ClinicalNeed { get; set; }

        [Required]
        [Display(Name = "Area")]
        public string AreaOfUsage { get; set; }

        [Required]
        [Display(Name = "Technology")]
        public string PotentialTechnology { get; set; }

        [Display(Name = "GMDN keywords")]
        public string Keywords { get; set; }
    }
}