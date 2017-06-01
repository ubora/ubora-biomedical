using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.ProjectCreation
{
    public class CreateProjectViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string ClinicalNeed { get; set; }

        [Required]
        public string AreaOfUsage { get; set; }

        [Required]
        public string PotentialTechnology { get; set; }

        public string Keywords { get; set; }
    }
}