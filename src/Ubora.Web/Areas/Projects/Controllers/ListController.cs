using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Projections;
using Ubora.Domain.Queries;
using Ubora.Web.Areas.Projects.Controllers.Shared;
using Ubora.Web.Areas.Projects.Views.List;

namespace Ubora.Web.Areas.Projects.Controllers
{
    public class ListController : ProjectsController
    {
        private readonly IQuery _query;

        public ListController(IQuery query)
        {
            _query = query;
        }

        [Route("projects")]
        public IActionResult List()
        {
            var projects = _query.Find<Project>();

            var viewModel = new ListViewModel
            {
                Projects = projects.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name })
            };

            return View(viewModel);
        }
    }
}