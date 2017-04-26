using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Web.Areas.Projects.Controllers.Shared;
using Ubora.Web.Areas.Projects.Views.Dashboard;

namespace Ubora.Web.Areas.Projects.Controllers
{
    public class DashboardController : ProjectsController
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

        [Route("projects/{id:guid}")]
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

        [HttpPost]
        public IActionResult DoWorkpackageOne(WorkPackageOneViewModel model)
        {
            var command = new EditProductSpecificationCommand
            {

            };
        }
    }
}