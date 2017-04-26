using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Web.Areas.Projects.Controllers.Shared;
using Ubora.Web.Areas.Projects.Views.Create;

namespace Ubora.Web.Areas.Projects.Controllers
{
    public class CreateController : ProjectsController
    {
        private readonly ICommandProcessor _commandProcessor;

        public CreateController(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        [HttpGet("projects/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("projects/create")]
        public IActionResult Create(CreatePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            var projectId = Guid.NewGuid();
            var command = new CreateProjectCommand
            {
                Id = projectId,
                Title = model.Title,
                Description = model.Description,
                AreaOfUsage = model.AreaOfUsage,
                GmdnCode = model.GmdnCode,
                ClinicalNeed = model.ClinicalNeed,
                GmdnDefinition = model.GmdnDefinition,
                GmdnTerm = model.GmdnTerm,
                PotentialTechnology = model.PotentialTechnology,
                UserInfo = new UserInfo(Guid.NewGuid(), "todo")
            };
            Execute(command);

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            return RedirectToAction("Dashboard", "Dashboard", new { id = projectId });
        }

        private void Execute(CreateProjectCommand createProjectCommand)
        {
            _commandProcessor.Execute(createProjectCommand);
        }
    }
}