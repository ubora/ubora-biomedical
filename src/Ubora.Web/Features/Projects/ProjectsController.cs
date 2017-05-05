using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;

namespace Ubora.Web.Features.Projects
{
    public class ProjectsController : ControllerBase
    {
        private readonly ICommandQueryProcessor _processor;
        private readonly IMapper _mapper;

        public ProjectsController(ICommandQueryProcessor processor, IMapper mapper)
        {
            _processor = processor;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        public IActionResult List()
        {
            var projects = _processor.Find<Project>();

            var viewModel = new ListViewModel
            {
                Projects = projects.Select(_mapper.Map<ListItemViewModel>)
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateViewModel model)
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

            return RedirectToAction("Index", "ProjectManagement", new { id = projectId });
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
