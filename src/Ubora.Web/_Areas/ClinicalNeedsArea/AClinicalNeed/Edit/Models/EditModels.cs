using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Edit.Models
{
    public class EditClinicalNeedViewModel : EditClinicalNeedPostModel
    {

    }

    public class EditClinicalNeedPostModel
    {
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string DescriptionQuillDelta { get; set; }

        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }
    }
}
