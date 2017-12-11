using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects._Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP1")]
    [WorkpackageStepIdFromRouteToViewData]
    public class WorkpackageOneController : ProjectController
    {
        private WorkpackageOne _workpackageOne;
        public WorkpackageOne WorkpackageOne => _workpackageOne ?? (_workpackageOne = QueryProcessor.FindById<WorkpackageOne>(ProjectId));

        [Route(nameof(ProjectOverview))]
        public IActionResult ProjectOverview(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var model = AutoMapper.Map<ProjectOverviewViewModel>(Project);

            return View(model);
        }

        public IActionResult DiscardDesignPlanningChanges(string returnUrl = null)
        {
            if(returnUrl != null)
            {
                return RedirectToLocal(returnUrl);
            }

            return RedirectToAction(nameof(ProjectOverview));
        }

        [HttpPost]
        [Route(nameof(ProjectOverview))]
        [Authorize(Policies.CanEditDesignPlanning)]
        public IActionResult ProjectOverview(ProjectOverviewViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return ProjectOverview(returnUrl);
            }

            ExecuteUserProjectCommand(new UpdateProjectCommand
            {
                AreaOfUsageTags = model.AreaOfUsageTags,
                ClinicalNeedTags = model.ClinicalNeedTags,
                PotentialTechnologyTags = model.PotentialTechnologyTags,
                Gmdn = model.Gmdn,
                Title = Project.Title
            });

            if (!ModelState.IsValid)
            {
                Notices.Error("Failed to change project overview!");

                return ProjectOverview(returnUrl);
            }

            Notices.Success("Project overview changed successfully!");

            if(returnUrl != null)
            {
                return RedirectToLocal(returnUrl);
            }

            return View();
        }

        [Route("{stepId}")]
        public IActionResult Read(string stepId)
        {
            var step = WorkpackageOne.GetSingleStep(stepId);
            var model = AutoMapper.Map<ReadStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });
            model.EditButton = GetEditButtonVisibility();

            UiElementVisibility GetEditButtonVisibility()
            {
                if (WorkpackageOne.HasReviewInProcess || WorkpackageOne.HasBeenAccepted)
                {
                    return UiElementVisibility.HiddenWithMessage("You can not edit work package when it's under review or has been accepted by review.");
                }
                return UiElementVisibility.Visible();
            }

            return View(model);
        }

        [Route("{stepId}/Edit")]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult Edit(string stepId)
        {
            var step = WorkpackageOne.GetSingleStep(stepId);

            var model = AutoMapper.Map<EditStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });

            return View(model);
        }

        [HttpPost]
        [Route("{stepId}/Edit")]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult Edit(EditStepPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            ExecuteUserProjectCommand(new EditWorkpackageOneStepCommand
            {
                StepId = model.StepId,
                NewValue = model.Content
            });

            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }
    }
}
