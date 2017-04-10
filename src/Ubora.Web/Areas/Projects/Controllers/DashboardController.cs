using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Projections;
using Ubora.Domain.Queries;
using Ubora.Web.Areas.Projects.Models.ProjectDashboard;

namespace Ubora.Web.Areas.Projects.Controllers
{
    [Area("Projects")]
    public class DashboardController : Controller
    {
        private readonly IQuery _query;
        private readonly IEventStreamQuery _eventStreamQuery;

        public DashboardController(IQuery query, IEventStreamQuery eventStreamQuery)
        {
            _query = query;
            _eventStreamQuery = eventStreamQuery;
        }

        public IActionResult Index(Guid id)
        {
            var project = _query.Load<Project>(id);

            var eventStream = _eventStreamQuery.Find(project.Id);

            var viewModel = new ProjectDashboardViewModel
            {
                EventStream = eventStream.Select(x => x.ToString()),
                Name = project.Name,
                Id = project.Id
            };

            return View(viewModel);
        }
    }
}