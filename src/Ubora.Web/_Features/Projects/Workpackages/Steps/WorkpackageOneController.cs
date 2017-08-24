using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP1")]
    public class WorkpackageOneController : ProjectController
    {
        private WorkpackageOne _workpackageOne;
        public WorkpackageOne WorkpackageOne => _workpackageOne ?? (_workpackageOne = QueryProcessor.FindById<WorkpackageOne>(ProjectId));

        [Route(nameof(ProjectOverview))]
        public IActionResult ProjectOverview()
        {
            var model = AutoMapper.Map<ProjectOverviewViewModel>(Project);

            return View(model);
        }

        [HttpPost]
        [Route(nameof(ProjectOverview))]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult ProjectOverview(ProjectOverviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ProjectOverview();
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
                var errorNotice = new Notice("Failed to change project overview!", NoticeType.Error);
                ShowNotice(errorNotice);

                return ProjectOverview();
            }

            var successNotice = new Notice("Project overview changed successfully!", NoticeType.Success);
            ShowNotice(successNotice);

            return View();
        }

        [Route(nameof(DeviceClassification))]
        public IActionResult DeviceClassification()
        {
            var model = AutoMapper.Map<DeviceClassificationViewModel>(Project);

            return View(nameof(DeviceClassification), model);
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
