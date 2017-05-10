using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Projects.Creation
{
    [Authorize]
    public class CreationController : ProjectController
    {
        private readonly ICommandQueryProcessor _processor;
        private readonly IMapper _mapper;

        public CreationController(ICommandQueryProcessor processor, IMapper mapper)
        {
            _processor = processor;
            _mapper = mapper;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var projectId = Guid.NewGuid();
            var command = new CreateProjectCommand
            {
                Id = projectId,
                UserInfo = UserInfo
            };
            _mapper.Map(model, command);

            _processor.Execute(command);

            return RedirectToAction("Dashboard", "Dashboard", new { id = projectId });
        }
    }
}
