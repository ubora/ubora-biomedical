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

        [Required]
        public string Gmdn { get; set; }

        [Obsolete]
        public string GmdnDefinition { get; set; }

        [Obsolete]
        public string GmdnCode { get; set; }
    }
}