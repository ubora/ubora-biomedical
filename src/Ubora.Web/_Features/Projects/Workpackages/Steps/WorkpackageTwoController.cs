﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages.Commands;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP2")]
    public class WorkpackageTwoController : ProjectController
    {
        private readonly IMapper _mapper;

        public WorkpackageTwoController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        protected Domain.Projects.Workpackages.WorkpackageTwo WorkpackageTwo => FindById<Domain.Projects.Workpackages.WorkpackageTwo>(ProjectId);

        [Route("{stepId}")]
        public IActionResult Read(string stepId)
        {
            var step = WorkpackageTwo.GetSingleStep(stepId);

            var model = _mapper.Map<StepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });

            return View(model);
        }

        [Route("{stepId}/Edit")]
        public IActionResult Edit(string stepId)
        {
            var step = WorkpackageTwo.GetSingleStep(stepId);

            var model = _mapper.Map<StepViewModel>(step);
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
    }
}