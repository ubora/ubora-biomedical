using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Edit.Models
{
    public class EditClinicalNeedViewModel : EditClinicalNeedPostModel
    {

    }

    public class EditClinicalNeedPostModel
    {
        [MaxLength(130, ErrorMessage = "Please shorten the title just a little bit.")]
        [MinLength(10, ErrorMessage = "Please provide atleast a bit longer title to identify the clinical need by.")]
        [Required(ErrorMessage = "Please specify a title to identify the clinical need by.")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "At least a short description is mandatory.")]
        [MinLength(length: 50, ErrorMessage = "Please describe the clinical need in richer detail.")]
        public string DescriptionQuillDelta { get; set; }

        [MaxLength(125)]
        public string ClinicalNeedTag { get; set; }
        [MaxLength(125)]
        public string AreaOfUsageTag { get; set; }
        [MaxLength(125)]
        public string PotentialTechnologyTag { get; set; }
        [MaxLength(125)]
        public string Keywords { get; set; }
    }
}
