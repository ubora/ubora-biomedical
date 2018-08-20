using System;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed._Shared
{
    public class LayoutViewModel
    {
        public string IndicatorProfilePictureUrl { get; set; }
        public Guid IndicatorUserId { get; set; }
        public DateTimeOffset IndicatedAt { get; set; }
        public string IndicatorFullName { get; set; }
        public ActiveTabOfClinicalNeed ActiveTab { get; set; }
        public int NumberOfRelatedProjects { get; set; }
        public int NumberOfComments { get; set; }
    }

    public enum ActiveTabOfClinicalNeed
    {
        Description,
        Comments,
        RelatedProjects
    }
}
