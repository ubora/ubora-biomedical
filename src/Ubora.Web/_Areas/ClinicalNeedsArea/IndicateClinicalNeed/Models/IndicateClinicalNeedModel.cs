using System.ComponentModel.DataAnnotations;
using Ubora.Domain;

namespace Ubora.Web._Areas.ClinicalNeedsArea.IndicateClinicalNeed.Models
{
    public class StepOneModel : ITagsAndKeywords
    {
        [MaxLength(130, ErrorMessage = "Please shorten the title just a little bit.")]
        [MinLength(10, ErrorMessage = "Please provide atleast a bit longer title to identify the clinical need by.")]
        [Required(ErrorMessage = "Please specify a title to identify the clinical need by.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "At least a short description is mandatory.")]
        [MinLength(length: 50, ErrorMessage = "Please describe the clinical need in richer detail.")]
        public string Description { get; set; }

        #region Hidden "step two" input for back and forward functionality

        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }

        #endregion

        public bool RestoreStepOneUrl { get; set; }
    }

    public class StepTwoModel : StepOneModel
    {
        public new string ClinicalNeedTag { get; set; }
        public new string AreaOfUsageTag { get; set; }
        public new string PotentialTechnologyTag { get; set; }
        public new string Keywords { get; set; }
    }
}