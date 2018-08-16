using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Queries;

namespace Ubora.Web._Areas.ClinicalNeedsArea.LandingPage
{
    public class LandingPageController : ClinicalNeedsAreaController
    {
        [Route("/clinical-needs")]
        public IActionResult LandingPage()
        {
            return View(QueryProcessor.ExecuteQuery(new ClinicalNeedsLandingPageQuery()));
        }
    }
}
