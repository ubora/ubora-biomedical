using System;
using Ubora.Domain;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview.Models
{
    public class OverviewViewModel : ITagsAndKeywords
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }
        public DateTimeOffset IndicatedAt  { get; set; }
    }
}
