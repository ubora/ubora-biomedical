using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Areas.Projects.Controllers.Shared;
using Ubora.Web.Areas.Projects.Views.List;

namespace Ubora.Web.Areas.Projects.Controllers
{
    public class ListController : ProjectsController
    {
        private readonly IQueryProcessor _query;

        public ListController(IQueryProcessor query)
        {
            _query = query;
        }

        [Route("projects")]
        public IActionResult List()
        {
            var projects = _query.Find<Project>();

            var viewModel = new ListViewModel
            {
                Projects = projects.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Title })
            };

            return View(viewModel);
        }
    }
}