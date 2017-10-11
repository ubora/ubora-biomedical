﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class ProjectOverviewViewModel
    {
        [Required]
        [Display(Name = "Clinical need")]
        public string ClinicalNeedTags { get; set; }

        [Required]
        [Display(Name = "Area")]
        public string AreaOfUsageTags { get; set; }

        [Required]
        [Display(Name = "Technology")]
        public string PotentialTechnologyTags { get; set; }

        [Display(Name = "Project keywords")]
        public string Gmdn { get; set; }
    }
}
