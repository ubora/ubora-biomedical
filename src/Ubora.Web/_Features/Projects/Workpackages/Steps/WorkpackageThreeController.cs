using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features.Projects._Shared;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP3")]
    [WorkpackageStepIdFromRouteToViewData]
    public class WorkpackageThreeController : ProjectController
    {
        private WorkpackageThree _workpackageThree;
        public WorkpackageThree WorkpackageThree => _workpackageThree ?? (_workpackageThree = QueryProcessor.FindById<WorkpackageThree>(ProjectId));

        [Route("{stepId}")]
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

        [Route(nameof(UnlockConfirmation))]
        public IActionResult UnlockConfirmation()
        {
            ViewBag.Title = "WP 3: Design and prototyping";
            ViewData["MenuOption"] = ProjectMenuOption.Workpackages;
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WorkpackageThreeLocked;
            
            return View(nameof(UnlockConfirmation));
        }
        
        [HttpPost]
        [Route(nameof(UnLock))]
        public IActionResult UnLock()
        {
            var command = new UnlockWorkpackageCommand { WorkpackageType = WorkpackageType.Three };
            ExecuteUserProjectCommand(command, Notice.Success("Unlocked."));
            
            return RedirectToAction("ProjectOverview","WorkpackageOne");
        }
    }
}
