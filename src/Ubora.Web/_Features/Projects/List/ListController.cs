using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Projects.List
{
    public class ListController : ProjectController
    {
        public IActionResult MyProjects()
        {
            return View();
        }
    }
}
