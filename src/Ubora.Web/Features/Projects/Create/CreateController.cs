using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;

namespace Ubora.Web.Features.Projects.Create
{
    public class CreateController : Controller
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IMapper _mapper;

        public CreateController(ICommandProcessor commandProcessor, IMapper mapper)
        {
            _commandProcessor = commandProcessor;
            _mapper = mapper;
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Create(CreatePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            var projectId = Guid.NewGuid();
            var command = new CreateProjectCommand
            {
                ProjectId = projectId,
                UserInfo = new UserInfo(Guid.NewGuid(), "")
            };
            _mapper.Map(model, command);

            Execute(command);

            return RedirectToAction("Dashboard", "Dashboard", new { id = projectId });
        }

        private void Execute(CreateProjectCommand createProjectCommand)
        {
            _commandProcessor.Execute(createProjectCommand);
        }
    }
}