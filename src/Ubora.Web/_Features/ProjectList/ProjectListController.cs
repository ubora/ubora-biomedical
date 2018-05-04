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

        public IActionResult Search([FromServices]ProjectListViewModel.Factory modelFactory, string title, int page = 1)
        {
            var model = modelFactory.CreateForSearch(title, page);

            return View(new SearchViewModel() { ProjectListViewModel = model });
        }

        [HttpPost]
        public IActionResult Search([FromServices]ProjectListViewModel.Factory modelFactory, SearchViewModel model)
        {
            var viewModel = modelFactory.CreateForSearch(model.Title, 1);

            return View(new SearchViewModel() { Title = model.Title, ProjectListViewModel = viewModel });
        }
    }
}