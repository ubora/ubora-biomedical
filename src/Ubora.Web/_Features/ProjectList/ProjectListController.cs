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

        public IActionResult Search(int page = 1)
        {
            return View(new SearchViewModel() { Page = page });
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel model)
        {
            return View(model);
        }
    }
}