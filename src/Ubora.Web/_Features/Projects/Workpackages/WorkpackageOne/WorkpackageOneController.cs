using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageOne
{
    public class WorkpackageOneController : ProjectController
    {
        private readonly IMapper _mapper;

        protected Domain.Projects.WorkpackageOnes.WorkpackageOne WorkpackageOne => this.FindById<Domain.Projects.WorkpackageOnes.WorkpackageOne>(ProjectId);

        public WorkpackageOneController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

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

        public IActionResult EditStep(Guid id)
        {
            var step = WorkpackageOne.GetSingleStep(id);

            var model = _mapper.Map<StepViewModel>(step);

            return View(model);
        }

        [HttpPost]
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
    }
}
