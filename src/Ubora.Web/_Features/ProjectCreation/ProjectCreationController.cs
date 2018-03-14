using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Authorization;
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
                ClinicalNeed = model.ClinicalNeedTags,
                AreaOfUsage = model.AreaOfUsageTags,
                PotentialTechnology = model.PotentialTechnologyTags,
                Gmdn = model.Gmdn
            }, Notice.Success(SuccessTexts.ProjectCreated));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Dashboard", "Dashboard", new { projectId = projectId });
        }
    }
}
