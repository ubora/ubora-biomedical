using System;
using Ubora.Domain;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview.Models
{
    public class OverviewViewModel : ITagsAndKeywords
    {
        public Guid Id { get; set; }
        public string DescriptionQuillDelta { get; set; }
        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }
    }
}
