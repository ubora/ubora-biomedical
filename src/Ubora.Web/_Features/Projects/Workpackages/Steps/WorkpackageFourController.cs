﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Specifications;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP4")]
    [WorkpackageStepIdFromRouteToViewData]
    public class WorkpackageFourController : ProjectController
    {
        public const string Name = "WorkpackageFour";

        private WorkpackageFour _workpackageFour;
        public WorkpackageFour WorkpackageFour =>
            _workpackageFour ?? (_workpackageFour = QueryProcessor.FindById<WorkpackageFour>(ProjectId));

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData[nameof(ProjectMenuOption)] = ProjectMenuOption.Workpackages;
            ViewData[nameof(PageTitle)] = "WP 4: Implementation";
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (WorkpackageFour == null) 
                return RedirectToAction(nameof(Unlocking));
            return RedirectToAction(nameof(Read), new { stepId = WorkpackageFour.Steps.First().Id });
        }

        [HttpGet("{stepId}")]
        public async Task<IActionResult> Read(string stepId)
        {
            var step = WorkpackageFour.GetSingleStep(stepId);
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
            var step = WorkpackageFour.GetSingleStep(stepId);

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

            ExecuteUserProjectCommand(new EditWorkpackageFourStepCommand
            {
                StepId = model.StepId,
                NewValue = new QuillDelta(model.ContentQuillDelta)
            }, Notice.Success(SuccessTexts.WP4StepEdited));

            if (!ModelState.IsValid)
            {
                return await Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }

        [HttpGet(nameof(StructuredInformationOnTheDevice))]
        public IActionResult StructuredInformationOnTheDevice([FromServices] StructuredInformationResultViewModel.Factory modelFactory)
        {
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WP4StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new DeviceStructuredInformationFromWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes.Four)&& new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = ProjectId })
                .FirstOrDefault();
            var model = modelFactory.Create(deviceStructuredInformation);

            return View(model);
        }

        [HttpGet(nameof(HealthTechnologySpecifications))]
        public virtual IActionResult HealthTechnologySpecifications([FromServices] HealthTechnologySpecificationsViewModel.Factory modelFactory)
        {
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WP4StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new DeviceStructuredInformationFromWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes.Four) && new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = ProjectId })
                .FirstOrDefault();
            if (deviceStructuredInformation == null)
            {
                return View(nameof(HealthTechnologySpecifications));
            }

            var model = modelFactory.Create(deviceStructuredInformation.HealthTechnologySpecification);
            model.DeviceStructuredInformationId = deviceStructuredInformation.Id;

            return View(nameof(HealthTechnologySpecifications), model);
        }

        [HttpPost(nameof(HealthTechnologySpecifications))]
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
            command.WorkpackageType = DeviceStructuredInformationWorkpackageTypes.Four;
            
            ExecuteUserProjectCommand(command, Notice.Success(SuccessTexts.WP4HealthTechnologySpecificationsEdited));

            if (!ModelState.IsValid)
            {
                return HealthTechnologySpecifications(modelFactory);
            }

            return RedirectToAction(nameof(StructuredInformationOnTheDevice));
        }

        [Route(nameof(UserAndEnvironment))]
        public virtual IActionResult UserAndEnvironment([FromServices] UserAndEnvironmentInformationViewModel.Factory modelFactory)
        {
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WP4StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new DeviceStructuredInformationFromWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes.Four) && new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = ProjectId })
                .FirstOrDefault();
            if (deviceStructuredInformation == null)
            {
                return View(nameof(UserAndEnvironment));
            }

            var model = modelFactory.Create(deviceStructuredInformation.UserAndEnvironment);
            model.DeviceStructuredInformationId = deviceStructuredInformation.Id;

            return View(nameof(UserAndEnvironment), model);
        }

        [HttpPost(nameof(UserAndEnvironment))]
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
            command.WorkpackageType = DeviceStructuredInformationWorkpackageTypes.Four;
            ExecuteUserProjectCommand(command , Notice.Success(SuccessTexts.WP4UserAndEnvironmentEdited));

            if (!ModelState.IsValid)
            {
                return UserAndEnvironment(modelFactory);
            }

            return RedirectToAction(nameof(StructuredInformationOnTheDevice));
        }

        [HttpGet("unlock")]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkpackages))]
        public IActionResult Unlocking()
        {
            ViewData[nameof(PageTitle)] = "WP 4: Implementation";
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WorkpackageFourLocked;

            return View("UnlockWp4");
        }

        [HttpPost("unlock")]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkpackages))]
        public IActionResult Unlock()
        {
            ExecuteUserProjectCommand(new OpenWorkpackageFourCommand(), Notice.Success(SuccessTexts.WPUnlocked));

            if (!ModelState.IsValid)
            {
                return Unlocking();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}