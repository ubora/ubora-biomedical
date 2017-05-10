using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Creation
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
        public string GmdnTerm { get; set; }

        [Obsolete]
        public string GmdnDefinition { get; set; }

        [Obsolete]
        public string GmdnCode { get; set; }
    }
}