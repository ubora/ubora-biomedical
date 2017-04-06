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
    public class ProjectListController : Controller
    {
        private readonly IQuery _query;

        public ProjectListController(IQuery query)
        {
            _query = query;
        }

        [Route("Projects")]
        public IActionResult All()
        {
            var projects = _query.Find<Project>();

            var viewModel = new ProjectListViewModel
            {
                Projects = projects.Select(x => new ProjectListItemViewModel { Name = x.Name })
            };

            return View(viewModel);
        }
    }
}