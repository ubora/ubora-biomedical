using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Specifications;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features.Projects._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP2")]
    [WorkpackageStepIdFromRouteToViewData]
    public class WorkpackageTwoController : ProjectController
    {
        private WorkpackageTwo _workpackageTwo;

        public WorkpackageTwo WorkpackageTwo => _workpackageTwo ?? (_workpackageTwo = QueryProcessor.FindById<WorkpackageTwo>(ProjectId));

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["MenuOption"] = ProjectMenuOption.Workpackages;
        }

        [Route("{stepId}")]
        public IActionResult Read(string stepId)
        {
            var step = WorkpackageTwo.GetSingleStep(stepId);

            var model = AutoMapper.Map<ReadStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });
            model.EditButton = UiElementVisibility.Visible();

            return View(model);
        }

        [Route("{stepId}/Edit")]
        public IActionResult Edit(string stepId)
        {
            var step = WorkpackageTwo.GetSingleStep(stepId);

            var model = AutoMapper.Map<EditStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });

            return View(model);
        }

        [HttpPost("{stepId}/Edit")]
        public IActionResult Edit(EditStepPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            ExecuteUserProjectCommand(new EditWorkpackageTwoStepCommand
            {
                StepId = model.StepId,
                NewValue = model.Content
            }, Notice.Success(SuccessTexts.WP2StepEdited));

            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }

        [Route(nameof(StructuredInformationOnTheDevice))]
        public IActionResult StructuredInformationOnTheDevice([FromServices] StructuredInformationResultViewModel.Factory modelFactory)
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new IsWorkpackageTypeDeviceStructuredInformationSpec(WorkpackageType.Two) && new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = ProjectId })
                .FirstOrDefault();

            var model = modelFactory.Create(deviceStructuredInformation);

            return View(model);
        }

        [Route(nameof(HealthTechnologySpecifications))]
        public virtual IActionResult HealthTechnologySpecifications([FromServices] HealthTechnologySpecificationsViewModel.Factory modelFactory)
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new IsWorkpackageTypeDeviceStructuredInformationSpec(WorkpackageType.Two) && new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = ProjectId })
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
            command.WorkpackageType = WorkpackageType.Two;
            
            ExecuteUserProjectCommand(command, Notice.Success(SuccessTexts.WP3HealthTechnologySpecificationsEdited));

            if (!ModelState.IsValid)
            {
                return HealthTechnologySpecifications(modelFactory);
            }

            return RedirectToAction(nameof(StructuredInformationOnTheDevice));
        }

        [Route(nameof(UserAndEnvironment))]
        public virtual IActionResult UserAndEnvironment([FromServices] UserAndEnvironmentInformationViewModel.Factory modelFactory)
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new IsWorkpackageTypeDeviceStructuredInformationSpec(WorkpackageType.Two) && new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = ProjectId })
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
            command.WorkpackageType = WorkpackageType.Two;
            ExecuteUserProjectCommand(command, Notice.Success(SuccessTexts.WP3UserAndEnvironmentEdited));

            if (!ModelState.IsValid)
            {
                return UserAndEnvironment(modelFactory);
            }

            return RedirectToAction(nameof(StructuredInformationOnTheDevice));
        }
    }
}
