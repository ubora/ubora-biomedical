﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.ProjectCreation
{
    [Authorize]
    public class ProjectCreationController : UboraController
    {
        public ProjectCreationController(ICommandQueryProcessor processor) : base(processor)
        {
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
            ExecuteUserCommand(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Title = model.Title,
                ClinicalNeed = model.ClinicalNeedTags,
                AreaOfUsage = model.AreaOfUsageTags,
                PotentialTechnology = model.PotentialTechnologyTags,
                Gmdn = model.Gmdn
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Dashboard", "Dashboard", new { projectId = projectId });
        }
    }
}
