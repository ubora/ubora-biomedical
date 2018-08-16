using System;
using Ubora.Domain;

namespace Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Models
{
    public class ClinicalNeedViewModel : ITagsAndKeywords
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ClinicalNeedTag { get; set; }
        public string AreaOfUsageTag { get; set; }
        public string PotentialTechnologyTag { get; set; }
        public string Keywords { get; set; }
        public int NumberOfRelatedProjects { get; set; }
        public string IndicatorFullName { get; set; }
        public string IndicatorUserId { get; set; }
        public int NumberOfComments { get; set; }
        public DateTimeOffset IndicatedAt { get; set; }
    }
}
