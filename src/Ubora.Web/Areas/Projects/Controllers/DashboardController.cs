using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Projections;
using Ubora.Domain.Queries;
using Ubora.Web.Areas.Projects.Controllers.Shared;
using Ubora.Web.Areas.Projects.Views.Dashboard;

namespace Ubora.Web.Areas.Projects.Controllers
{
    public class DashboardController : ProjectsController
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IEventStreamQuery _eventStreamQuery;

        public DashboardController(IQueryProcessor queryProcessor, IEventStreamQuery eventStreamQuery)
        {
            _queryProcessor = queryProcessor;
            _eventStreamQuery = eventStreamQuery;
        }

        [Route("projects/{id:guid}")]
        public IActionResult Dashboard(Guid id)
        {
            var project = _queryProcessor.FindById<Project>(id);

            var eventStream = _eventStreamQuery.Find(project.Id);

            var viewModel = new DashboardViewModel
            {
                EventStream = eventStream.Select(x => x.ToString()),
                Name = project.Name,
                Id = project.Id,
                //Members = project.Members.Select(x => x.)
            };

            return View(viewModel);
        }
    }
}