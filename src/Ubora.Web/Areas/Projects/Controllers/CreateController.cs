using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Commands;
using Ubora.Domain.Events;
using Ubora.Domain.Projects;
using Ubora.Web.Areas.Projects.Controllers.Shared;
using Ubora.Web.Areas.Projects.Views.Create;

namespace Ubora.Web.Areas.Projects.Controllers
{
    public class CreateController : ProjectsController
    {
        private readonly ICommandBus _commandBus;

        public CreateController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
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

            var command = new CreateProjectCommand
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                UserInfo = new UserInfo(Guid.NewGuid(), "todo")
            };
            Execute(command);

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            return RedirectToAction("List", "List");
        }

        private void Execute(CreateProjectCommand createProjectCommand)
        {
            _commandBus.Execute(createProjectCommand);
        }
    }
}