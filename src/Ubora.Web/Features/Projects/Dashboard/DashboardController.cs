using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Web.Features.Projects.Dashboard
{
    public class DashboardController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IEventStreamQuery _eventStreamQuery;
        private readonly ICommandProcessor _commandProcessor;

        public DashboardController(IQueryProcessor queryProcessor, IEventStreamQuery eventStreamQuery, ICommandProcessor commandProcessor)
        {
            _queryProcessor = queryProcessor;
            _eventStreamQuery = eventStreamQuery;
            _commandProcessor = commandProcessor;
        }

        public IActionResult Dashboard(Guid id)
        {
            var project = _queryProcessor.FindById<Project>(id);

            var eventStream = _eventStreamQuery.Find(project.Id);

            var viewModel = new DashboardViewModel
            {
                EventStream = eventStream.Select(x => x.ToString()),
                Name = project.Title,
                Id = project.Id,
                ProjectData = Newtonsoft.Json.JsonConvert.SerializeObject(project)
            };

            return View(viewModel);
        }
    }
}