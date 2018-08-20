using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.Projects._Commands;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.ProjectCreation
{
    [Authorize(Policy = nameof(Policies.CanCreateProject))]
    public class ProjectCreationController : UboraController
    {
        public IActionResult Create(Guid? clinicalNeedId = null)
        {
            if (clinicalNeedId.HasValue)
            {
                var clinicalNeed = QueryProcessor.FindById<ClinicalNeed>(clinicalNeedId.Value);
                if (clinicalNeed == null)
                {
                    return NotFound();
                }

                return View(new CreateProjectViewModel
                {
                    ClinicalNeedId = clinicalNeedId,
                    ClinicalNeedTitle = clinicalNeed.Title,
                    PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                    AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                    ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                    Keywords = clinicalNeed.Keywords
                });
            }

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
                RelatedClinicalNeedId = model.ClinicalNeedId,
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
