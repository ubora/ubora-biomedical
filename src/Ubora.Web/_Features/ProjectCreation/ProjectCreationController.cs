using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects._Commands;
using Ubora.Web._Features._Shared.Notices;

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
                ClinicalNeedTag = model.ClinicalNeedTag,
                AreaOfUsageTag = model.AreaOfUsageTag,
                PotentialTechnologyTag = model.PotentialTechnologyTag,
                Keywords = model.Keywords
            }, Notice.None("Visual feedback obvious enough."));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Dashboard", "Dashboard", new { projectId = projectId });
        }
    }
}
