using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Specifications;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects.Workpackages.Queries;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web.Infrastructure;
using Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP4")]
    [WorkpackageStepIdFromRouteToViewData]
    public class WorkpackageFourController : ProjectController
    {
        private WorkpackageFour _workpackageFour;
        public WorkpackageFour WorkpackageFour =>
            _workpackageFour ?? (_workpackageFour = QueryProcessor.FindById<WorkpackageFour>(ProjectId));


        private readonly ViewRender _viewRender;
        private readonly IWordProcessingDocumentConverter _wordProcessingDocumentConverter;
        
        public WorkpackageFourController(ViewRender viewRender, IWordProcessingDocumentConverter wordProcessingDocumentConverter)
        {
            _viewRender = viewRender;
            _wordProcessingDocumentConverter = wordProcessingDocumentConverter;
        }
            
        
        public IActionResult FirstStep()
        {
            return RedirectToAction(nameof(Read), new { stepId = WorkpackageFour.Steps.First().Id });
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData[nameof(ProjectMenuOption)] = ProjectMenuOption.Workpackages;
        }

        [Route("{stepId}")]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
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

        [Route("{stepId}/Edit")]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
        public async Task<IActionResult> Edit(string stepId)
        {
            var step = WorkpackageFour.GetSingleStep(stepId);

            var model = AutoMapper.Map<EditStepViewModel>(step);
            model.ContentQuillDelta = await SanitizeQuillDeltaForEditing(step.ContentV2);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });

            return View(model);
        }

        [HttpPost]
        [Route("{stepId}/Edit")]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
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
            }, Notice.Success("Changes saved"));

            if (!ModelState.IsValid)
            {
                return await Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }

        [Route(nameof(StructuredInformationOnTheDevice))]
        [Authorize(Policy = nameof(Policies.CanEditAndViewUnlockedWorkPackageFour))]
        public IActionResult StructuredInformationOnTheDevice([FromServices] StructuredInformationResultViewModel.Factory modelFactory)
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.WP4StructuredInformationOnTheDevice;

            var deviceStructuredInformation = QueryProcessor
                .Find(new DeviceStructuredInformationFromWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes.Four)&& new IsFromProjectSpec<DeviceStructuredInformation> { ProjectId = ProjectId })
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
            command.WorkpackageType = DeviceStructuredInformationWorkpackageTypes.Four;
            
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
            command.WorkpackageType = DeviceStructuredInformationWorkpackageTypes.Four;
            ExecuteUserProjectCommand(command , Notice.Success("WP4UserAndEnvironmentEdited"));

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
            ViewBag.Title = "WP 4: Implementation";
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WorkpackageFourLocked;

            return View("UnlockWp4");
        }

        [HttpPost("unlock")]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkpackages))]
        public IActionResult Unlock()
        {
            ExecuteUserProjectCommand(new OpenWorkpackageFourCommand(), Notice.Success("Work package unlocked"));

            if (!ModelState.IsValid)
            {
                return Unlocking();
            }

            return RedirectToAction(nameof(FirstStep));
        }
        
        [Route("preproduction-document")]
        public IActionResult PreproductionDocument()
        { 
            ViewBag.Title = "WP 4: Implementation";
            ViewData["MenuOption"] = ProjectMenuOption.Workpackages;
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.PreproductionDocuments;

            var wpStatuses = QueryProcessor.ExecuteQuery(new GetStatusesOfProjectWorkpackagesQuery(ProjectId));
            
            var model = new PreproductionDocumentsViewModel
            {
                WorkpackageCheckBoxListItems = new List<WorkpackageCheckBoxListItem>
                {
                    new WorkpackageCheckBoxListItem { Name = "WP1", IsExportable = wpStatuses.Wp1Status == WorkpackageStatus.Opened || wpStatuses.Wp1Status == WorkpackageStatus.Accepted},
                    new WorkpackageCheckBoxListItem { Name = "WP2", IsExportable = wpStatuses.Wp2Status == WorkpackageStatus.Opened || wpStatuses.Wp2Status == WorkpackageStatus.Accepted},
                    new WorkpackageCheckBoxListItem { Name = "WP3", IsExportable = wpStatuses.Wp3Status == WorkpackageStatus.Opened || wpStatuses.Wp3Status == WorkpackageStatus.Accepted},
                    new WorkpackageCheckBoxListItem { Name = "WP4", IsExportable = wpStatuses.Wp4Status == WorkpackageStatus.Opened || wpStatuses.Wp4Status == WorkpackageStatus.Accepted}
                }
            };
            
            return View("PreproductionDocuments/PreproductionDocument", model);
        }
        
        [HttpPost]
        [Route("preproduction-document")]
        public async Task<IActionResult> DownloadPreproductionDocument(PreproductionDocumentsViewModel model, [FromServices] PreproductionDocumentTemplateViewModel.Factory modelFactory)
        {
            var preproductionDocumentTemplateViewModel = await modelFactory.Create(Project);

            var view = _viewRender.Render("/_Features/Projects/Workpackages/Steps/PreproductionDocuments/", "PreproductionDocumentTemplate.cshtml", preproductionDocumentTemplateViewModel);
            var documentStream = await _wordProcessingDocumentConverter.GetDocumentStreamAsync(view);
            
            return File(documentStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Preproduction_document_{DateTime.UtcNow}.docx");
        }
    }
}