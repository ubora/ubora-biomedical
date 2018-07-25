using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features.Projects._Shared;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP3")]
    [WorkpackageStepIdFromRouteToViewData]
    public class WorkpackageThreeController : ProjectController
    {
        private WorkpackageThree _workpackageThree;
        public WorkpackageThree WorkpackageThree => _workpackageThree ?? (_workpackageThree = QueryProcessor.FindById<WorkpackageThree>(ProjectId));

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["MenuOption"] = ProjectMenuOption.Workpackages;
        }

        public IActionResult FirstStep()
        {
            return RedirectToAction(nameof(Read), new { stepId = WorkpackageThree.Steps.First().Id });
        }

        [Route("{stepId}")]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageThree))]
        public IActionResult Read(string stepId)
        {
            var step = WorkpackageThree.GetSingleStep(stepId);
            var model = AutoMapper.Map<ReadStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });
            model.EditButton = UiElementVisibility.Visible();

            return View(model);
        }

        [Route("{stepId}/Edit")]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageThree))]
        public IActionResult Edit(string stepId)
        {
            var step = WorkpackageThree.GetSingleStep(stepId);

            var model = AutoMapper.Map<EditStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });

            return View(model);
        }

        [HttpPost]
        [Route("{stepId}/Edit")]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageThree))]
        public IActionResult Edit(EditStepPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            ExecuteUserProjectCommand(new EditWorkpackageThreeStepCommand
            {
                StepId = model.StepId,
                NewValue = model.Content
            }, Notice.Success(SuccessTexts.WP3StepEdited));

            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }

        [Route(nameof(Unlocking))]
        public IActionResult Unlocking()
        {
            ViewBag.Title = "WP 3: Design and prototyping";
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WorkpackageThreeLocked;

            return View(nameof(Unlocking));
        }

        [HttpPost]
        [Route(nameof(Unlock))]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkpackages))]
        public IActionResult Unlock()
        {
            ExecuteUserProjectCommand(new OpenWorkpackageThreeCommand(), Notice.Success("Work package unlocked"));

            if (!ModelState.IsValid)
            {
                return Unlocking();
            }

            return RedirectToAction(nameof(FirstStep));
        }
    }
}
