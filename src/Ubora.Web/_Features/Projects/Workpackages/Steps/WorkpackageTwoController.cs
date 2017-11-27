﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features.Projects._Shared;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP2")]
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
            });

            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }

        [Route(nameof(StructuredInformationOnTheDevice))]
        public IActionResult StructuredInformationOnTheDevice()
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.StructuredInformationOnTheDevice;

            return View();
        }

        [Route(nameof(HealthTechnologySpecifications))]
        public IActionResult HealthTechnologySpecifications()
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.StructuredInformationOnTheDevice;

            var model = new HealthTechnologySpecificationsViewModel
            {
                
            };

            return View(model);
        }

        [HttpPost]
        [Route(nameof(HealthTechnologySpecifications))]
        public IActionResult HealthTechnologySpecifications(HealthTechnologySpecificationsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HealthTechnologySpecifications();
            }

            return RedirectToAction(nameof(StructuredInformationOnTheDeviceResult));
        }

        [Route(nameof(UserAndEnvironment))]
        public virtual IActionResult UserAndEnvironment()
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.StructuredInformationOnTheDevice;

            var model = new UserAndEnvironmentInformationViewModel
            {
            };

            return View(model);
        }

        [HttpPost]
        [Route(nameof(UserAndEnvironment))]
        public IActionResult EditUserAndEnvironment(
            UserAndEnvironmentInformationViewModel model, 
            [FromServices] UserAndEnvironmentInformationViewModel.Mapper modelMapper)
        {
            if (!ModelState.IsValid)
            {
                return UserAndEnvironment();
            }

            var command = modelMapper.MapToCommand(model);
            ExecuteUserProjectCommand(command);

            if (!ModelState.IsValid)
            {
                return UserAndEnvironment();
            }

            return RedirectToAction(nameof(StructuredInformationOnTheDeviceResult));
        }

        [Route(nameof(StructuredInformationOnTheDeviceResult))]
        public IActionResult StructuredInformationOnTheDeviceResult()
        {
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.StructuredInformationOnTheDevice;

            return View();
        }
    }
}
