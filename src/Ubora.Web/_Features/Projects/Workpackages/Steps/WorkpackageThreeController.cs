using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features.Projects._Shared;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain;

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
        public async Task<IActionResult> Read(string stepId)
        {
            var step = WorkpackageThree.GetSingleStep(stepId);
            var model = AutoMapper.Map<ReadStepViewModel>(step);
            model.ContentHtml = await ConvertQuillDeltaToHtml(step.ContentV2);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });
            model.EditButton = UiElementVisibility.Visible();

            return View(model);
        }

        [Route("{stepId}/Edit")]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageThree))]
        public async Task<IActionResult> Edit(string stepId)
        {
            var step = WorkpackageThree.GetSingleStep(stepId);

            var model = AutoMapper.Map<EditStepViewModel>(step);
            model.ContentQuillDelta = await SanitizeQuillDeltaForEditing(step.ContentV2);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });

            return View(model);
        }

        [HttpPost]
        [Route("{stepId}/Edit")]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageThree))]
        public async Task<IActionResult> Edit(EditStepPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Edit(model.StepId);
            }

            ExecuteUserProjectCommand(new EditWorkpackageThreeStepCommand
            {
                StepId = model.StepId,
                NewValue = new QuillDelta(model.ContentQuillDelta)
            }, Notice.Success(SuccessTexts.WP3StepEdited));

            if (!ModelState.IsValid)
            {
                return await Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }

        [Route(nameof(Unlocking))]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkpackages))]
        public IActionResult Unlocking()
        {
            ViewBag.Title = "WP 3: Design and prototyping";
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WorkpackageThreeLocked;

            return View("UnlockWp3");
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
