using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP4")]
    [WorkpackageStepIdFromRouteToViewData]
    public class WorkpackageFourController : ProjectController
    {
        private WorkpackageFour _workpackageFour;

        public WorkpackageFour WorkpackageFour =>
            _workpackageFour ?? (_workpackageFour = QueryProcessor.FindById<WorkpackageFour>(ProjectId));

        [Route("{stepId}")]
        public IActionResult Read(string stepId)
        {
            var step = WorkpackageFour.GetSingleStep(stepId);
            var model = AutoMapper.Map<ReadStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });
            model.EditButton = UiElementVisibility.Visible();

            return View(model);
        }

        [Route("{stepId}/Edit")]
        public IActionResult Edit(string stepId)
        {
            var step = WorkpackageFour.GetSingleStep(stepId);
            
            var model = AutoMapper.Map<EditStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new {stepId});
            model.ReadStepUrl = Url.Action(nameof(Read), new {stepId});

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

            ExecuteUserProjectCommand(new EditWorkpackageFourStepCommand
            {
                StepId = model.StepId,
                NewValue = model.Content
            }, Notice.Success("WP4StepEdited"));

            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new {stepId = model.StepId});
        }
        
        [Route(nameof(StructuredInformationOnTheDevice))]
        public IActionResult StructuredInformationOnTheDevice([FromServices] StructuredInformationResultViewModel.Factory modelFactory)
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.WP4StructuredInformationOnTheDevice;

/*            var deviceStructuredInformation = QueryProcessor.FindById<DeviceStructuredInformation>(ProjectId);

            var model = modelFactory.Create(deviceStructuredInformation);*/

            var model = new StructuredInformationResultViewModel
            {
                UserAndEnvironment = new UserAndEnvironmentResult(),
                HealthTechnologySpecifications = new HealthTechnologySpecificationsResult(),
                IsUserAndEnvironmentEdited = false,
                IsHealthTechnologySpecificationEdited = false
            };

            return View(model);
        }
    }
}