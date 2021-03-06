﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP6")]
    [WorkpackageStepIdFromRouteToViewData]
    public class WorkpackageSixController : ProjectController
    {
        public const string Name = "WorkpackageSix";

        private WorkpackageSix _workpackageSix;
        public WorkpackageSix WorkpackageSix =>
            _workpackageSix ?? (_workpackageSix = QueryProcessor.FindById<WorkpackageSix>(ProjectId));

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData[nameof(ProjectMenuOption)] = ProjectMenuOption.Workpackages;
            ViewData[nameof(PageTitle)] = "WP 6: Project closure";
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (WorkpackageSix == null)
                return RedirectToAction(nameof(Unlocking));
            return RedirectToAction(nameof(Read), new { stepId = WorkpackageSix.Steps.First().Id });
        }

        [HttpGet("unlock")]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkpackages))]
        public IActionResult Unlocking()
        {
            if (WorkpackageSix != null) 
                return RedirectToAction(nameof(Index));

            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WorkpackageSixLocked;
            return View("UnlockWp6");
        }

        [HttpPost("unlock")]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkpackages))]
        public IActionResult Unlock()
        {
            ExecuteUserProjectCommand(new OpenWorkpackageSixCommand(), Notice.Success(SuccessTexts.WPUnlocked));

            if (!ModelState.IsValid)
            {
                return Unlocking();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{stepId}")]
        public async Task<IActionResult> Read(string stepId)
        {
            var step = WorkpackageSix.GetSingleStep(stepId);
            var model = AutoMapper.Map<ReadStepViewModel>(step);
            model.ContentHtml = await ConvertQuillDeltaToHtml(step.ContentV2);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });
            model.EditButton = UiElementVisibility.Visible();
            return View(model);
        }

        [HttpGet("{stepId}/Edit")]
        public async Task<IActionResult> Edit(string stepId)
        {
            var step = WorkpackageSix.GetSingleStep(stepId);
            var model = AutoMapper.Map<EditStepViewModel>(step);
            model.ContentQuillDelta = await SanitizeQuillDeltaForEditing(step.ContentV2);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });
            return View(model);
        }

        [HttpPost("{stepId}/Edit")]
        public async Task<IActionResult> Edit(EditStepPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Edit(model.StepId);
            }

            ExecuteUserProjectCommand(new EditWorkpackageSixStepCommand
            {
                StepId = model.StepId,
                NewValue = new QuillDelta(model.ContentQuillDelta)
            }, Notice.Success(SuccessTexts.WP6StepEdited));

            if (!ModelState.IsValid)
            {
                return await Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }
    }
}
