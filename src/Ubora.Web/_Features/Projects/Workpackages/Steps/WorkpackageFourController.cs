using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Specifications;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Projects._Shared;
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
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
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
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
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
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
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
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
        public IActionResult StructuredInformationOnTheDevice([FromServices] StructuredInformationResultViewModel.Factory modelFactory)
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.WP4StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new IsProjectAndWorkpackageTypeDeviceStructuredInformationSpec(ProjectId, WorkpackageType.Four))
                .FirstOrDefault();
            var model = modelFactory.Create(deviceStructuredInformation);

            return View(model);
        }
        
        [Route(nameof(HealthTechnologySpecifications))]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
        public virtual IActionResult HealthTechnologySpecifications([FromServices] HealthTechnologySpecificationsViewModel.Factory modelFactory)
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.WP4StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new IsProjectAndWorkpackageTypeDeviceStructuredInformationSpec(ProjectId, WorkpackageType.Four))
                .FirstOrDefault();
            if (deviceStructuredInformation == null)
            {
                return View(nameof(HealthTechnologySpecifications));
            }

            var model = modelFactory.Create(deviceStructuredInformation.HealthTechnologySpecification);
            model.DeviceStructuredInformationId = deviceStructuredInformation.Id;

            return View(nameof(HealthTechnologySpecifications),model);
        }
        
        [HttpPost]
        [Route(nameof(HealthTechnologySpecifications))]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
        public IActionResult EditHealthTechnologySpecifications(
            HealthTechnologySpecificationsViewModel model,
            [FromServices] HealthTechnologySpecificationsViewModel.Mapper modelMapper,
            [FromServices] HealthTechnologySpecificationsViewModel.Factory modelFactory)
        {
            if (!ModelState.IsValid)
            {
                return HealthTechnologySpecifications(modelFactory);
            }

            var command = modelMapper.MapToCommand(model);
            command.DeviceStructuredInformationId = model.DeviceStructuredInformationId;
            command.WorkpackageType = WorkpackageType.Four;
            
            ExecuteUserProjectCommand(command, Notice.Success("WP4HealthTechnologySpecificationsEdited"));

            if (!ModelState.IsValid)
            {
                return HealthTechnologySpecifications(modelFactory);
            }

            return RedirectToAction(nameof(StructuredInformationOnTheDevice));
        }
        
        [Route(nameof(UserAndEnvironment))]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
        public virtual IActionResult UserAndEnvironment([FromServices] UserAndEnvironmentInformationViewModel.Factory modelFactory)
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.WP4StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new IsProjectAndWorkpackageTypeDeviceStructuredInformationSpec(ProjectId, WorkpackageType.Four))
                .FirstOrDefault();
            if (deviceStructuredInformation == null)
            {
                return View(nameof(UserAndEnvironment));
            }

            var model = modelFactory.Create(deviceStructuredInformation.UserAndEnvironment);
            model.DeviceStructuredInformationId = deviceStructuredInformation.Id;

            return View(nameof(UserAndEnvironment),model);
        }

        [HttpPost]
        [Route(nameof(UserAndEnvironment))]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
        public IActionResult EditUserAndEnvironment(
            UserAndEnvironmentInformationViewModel model, 
            [FromServices] UserAndEnvironmentInformationViewModel.Mapper modelMapper,
            [FromServices] UserAndEnvironmentInformationViewModel.Factory modelFactory)
        {
            if (!ModelState.IsValid)
            {
                return UserAndEnvironment(modelFactory);
            }

            var command = modelMapper.MapToCommand(model);
            command.DeviceStructuredInformationId = model.DeviceStructuredInformationId;
            command.WorkpackageType = WorkpackageType.Four;
            ExecuteUserProjectCommand(command , Notice.Success("WP4UserAndEnvironmentEdited"));

            if (!ModelState.IsValid)
            {
                return UserAndEnvironment(modelFactory);
            }

            return RedirectToAction(nameof(StructuredInformationOnTheDevice));
        }
        
        [Route(nameof(UnlockConfirmation))]
        public IActionResult UnlockConfirmation()
        {
            ViewBag.Title = "WP 4: Implementation";
            ViewData["MenuOption"] = ProjectMenuOption.Workpackages;
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WorkpackageFourLocked;
            
            return View(nameof(UnlockConfirmation));
        }
        
        [HttpPost]
        [Route(nameof(Unlock))]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkPackage))]
        public IActionResult Unlock()
        {
            var command = new UnlockWorkpackageCommand { WorkpackageType = WorkpackageType.Four };
            ExecuteUserProjectCommand(command, Notice.Success("Unlocked."));
            
            return RedirectToAction("ProjectOverview","WorkpackageOne");
        }
    }
}