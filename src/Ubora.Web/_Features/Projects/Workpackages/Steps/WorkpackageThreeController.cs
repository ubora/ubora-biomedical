using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features._Shared;

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
            });

            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }
    }
}
