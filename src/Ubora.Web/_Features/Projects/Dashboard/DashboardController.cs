﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class DashboardController : ProjectController
    {
        private readonly IMapper _mapper;

        public DashboardController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        public IActionResult Dashboard()
        {
            var model = _mapper.Map<ProjectDashboardViewModel>(Project);

            return View(model);
        }

        public IActionResult EditProjectDescription()
        {
            var editProjectDescription = new EditProjectDescriptionViewModel
            {
                ProjectDescription = Project.Description
            };

            return View(editProjectDescription);
        } 

        [HttpPost]
        public IActionResult EditProjectDescription(EditProjectDescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditProjectDescription();
            }

            ExecuteUserProjectCommand(new UpdateProjectDescriptionCommand
            {
                ProjectId = ProjectId,
                Description = model.ProjectDescription
            });

            if (!ModelState.IsValid)
            {
                return EditProjectDescription();
            }

            return RedirectToAction(nameof(Dashboard));
        }
    }
}