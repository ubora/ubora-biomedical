using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Projects.Workpackages
{
    [ProjectRoute("[controller]")]
    public class WorkpackagesController : ProjectController
    {
        public IActionResult OverviewOfAll()
        {
            return View();
        }
    }
}