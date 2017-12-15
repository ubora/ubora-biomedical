using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Authorization;

namespace Ubora.Web._Features.ProjectCreation
{
    [Authorize(Policy = nameof(Policies.CanCreateProject))]
    public class ProjectCreationController : UboraController
    {
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

            Notices.Success("Project created successfully!");

            return RedirectToAction("Dashboard", "Dashboard", new { projectId = projectId });
        }
    }
}
