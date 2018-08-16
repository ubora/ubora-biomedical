using System;
using Ubora.Domain;
using Ubora.Web._Components.Discussions.Models;

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
        public DiscussionViewModel Discussion { get; set; }
        public Guid IndicatorUserId { get; set; }
        public string IndicatorFullName { get; set; }
        public string IndicatorProfilePictureUrl { get; set; }
    }
}
