using System.Collections.Generic;

namespace Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Models
{
    public class LandingPageViewModel
    {
        public IReadOnlyCollection<ClinicalNeedViewModel> ClinicalNeeds { get; set; }
    }
}
