using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Web.Features.ProjectManagement
{
    [Authorize]
    public class ProjectManagementController : Controller
    {
        private readonly ICommandQueryProcessor _processor;
        private readonly IEventStreamQuery _eventStreamQuery;

        public ProjectManagementController(ICommandQueryProcessor processor, IEventStreamQuery eventStreamQuery)
        {
            _processor = processor;
            _eventStreamQuery = eventStreamQuery;
        }

        public IActionResult Index(Guid id)
        {
            return RedirectToAction(nameof(Dashboard), new { id });
        }

        public IActionResult Dashboard(Guid id)
        {
            var project = _processor.FindById<Project>(id);

            var model = new DashboardViewModel
            {
                Name = project.Title,
                Id = project.Id
            };

            return View(model);
        }

        public IActionResult History(Guid id)
        {
            var projectEventStream = _eventStreamQuery.Find(id);

            var model = new HistoryViewModel
            {
                Events = projectEventStream.Select(x => x.ToString())
            };

            return View(model);
        }
    }
}
