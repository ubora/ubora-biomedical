using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Commands;
using Ubora.Domain.Events;
using Ubora.Domain.Projects;
using Ubora.Web.Areas.Projects.Models.ProjectCreation;

namespace Ubora.Web.Areas.Projects.Controllers
{
    [Area("Projects")]
    public class CreateController : Controller
    {
        private readonly ICommandProcessor _commandProcessor;

        public CreateController(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProjectCreationPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
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
                return View("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        private void Execute(CreateProjectCommand createProjectCommand)
        {
            _commandProcessor.Execute(createProjectCommand);
        }
    }
}