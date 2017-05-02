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

            var eventStream = _eventStreamQuery.Find(project.Id);

            var viewModel = new DashboardViewModel
            {
                EventStream = eventStream.Select(x => x.ToString()),
                Name = project.Title,
                Id = project.Id,
            };

            return View(viewModel);
        }
    }
}
