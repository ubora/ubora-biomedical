using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.ProjectList
{
    public class ProjectListController : UboraController
    {
        public ProjectListController(ICommandQueryProcessor processor) : base(processor)
        {
        }

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