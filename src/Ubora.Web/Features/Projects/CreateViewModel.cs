using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Features.Projects
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string ClinicalNeed { get; set; }

        public string AreaOfUsage { get; set; }

        public string PotentialTechnology { get; set; }

        public string GmdnTerm { get; set; }

        public string GmdnDefinition { get; set; }

        public string GmdnCode { get; set; }
    }
}