using System.Collections.Generic;
using Ubora.Web._Features._Shared.Paging;

namespace Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Models
{
    public class ClinicalNeedCardsViewComponentViewModel
    {
        public IReadOnlyCollection<ClinicalNeedCardViewModel> ClinicalNeedCards { get; set; }
        public bool ShowPager { get; set; }
        public Pager Pager { get; set; }
    }
}
