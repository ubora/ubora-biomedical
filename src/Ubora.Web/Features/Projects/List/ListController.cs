using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Web.Features.Projects.List
{
    public class ListController : Controller
    {
        private readonly IQueryProcessor _query;

        public ListController(IQueryProcessor query)
        {
            _query = query;
        }

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