using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.ProjectList
{
    public class ProjectListController : UboraController
    {
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