using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Projections;
using Ubora.Domain.Queries;
using Ubora.Web.Areas.Projects.Models.ProjectList;

namespace Ubora.Web.Areas.Projects.Controllers
{
    [Area("Projects")]
    public class HomeController : Controller
    {
        private readonly IQuery _query;

        public HomeController(IQuery query)
        {
            _query = query;
        }

        public IActionResult Index()
        {
            var projects = _query.Find<Domain.Projects.Projections.Project>();

            var viewModel = new ProjectListViewModel
            {
                Projects = projects.Select(x => new ProjectListItemViewModel { Id = x.Id, Name = x.Name })
            };

            return View(viewModel);
        }
    }
}