using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageOne
{
    public class WorkpackageOneController : ProjectController
    {
        private readonly IMapper _mapper;

        public WorkpackageOneController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        protected Domain.Projects.Workpackages.WorkpackageOne WorkpackageOne => FindById<Domain.Projects.Workpackages.WorkpackageOne>(ProjectId);

        public IActionResult Overview()
        {
            return View();
        }

        public IActionResult Step(Guid id)
        {
            var step = WorkpackageOne.GetSingleStep(id);

            var model = _mapper.Map<StepViewModel>(step); 

            return View(model);
        }

        // TODO: Hide in UI
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult EditStep(Guid id)
        {
            var step = WorkpackageOne.GetSingleStep(id);

            var model = _mapper.Map<StepViewModel>(step);

            return View(model);
        }

        [HttpPost]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult EditStep(EditStepPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditStep(model.StepId);
            }

            ExecuteUserProjectCommand(new EditWorkpackageOneStepCommand
            {
                StepId = model.StepId,
                NewValue = model.Content
            });

            if (!ModelState.IsValid)
            {
                return EditStep(model.StepId);
            }

            return RedirectToAction(nameof(Step), new { id = model.StepId });
        }

        // TODO
        [HttpPost]
        public IActionResult CommentStep(object model)
        {
            return null;
        }
    }
}
