using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Models;

namespace Ubora.Web._Areas.ClinicalNeedsArea.LandingPage
{
    public class LandingPageController : ClinicalNeedsAreaController
    {
        [Route("/clinical-needs")]
        public IActionResult LandingPage()
        {
            return View(new LandingPageViewModel());
        }
    }
}
