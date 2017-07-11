using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackagesController : ProjectController
    {
        public IActionResult OverviewOfAll()
        {
            return View();
        }
    }
}