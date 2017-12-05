using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.ProjectList
{
    public class ProjectListController : UboraController
    {
        [Authorize]
        public IActionResult MyProjects()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel model)
        {
            return View(model);
        }
    }
}