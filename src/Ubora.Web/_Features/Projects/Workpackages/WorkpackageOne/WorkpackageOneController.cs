using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageOne
{
    public class WorkpackageOneController : ProjectController
    {
        public Domain.Projects.WorkpackageOnes.WorkpackageOne WorkpackageOne => this.FindById<Domain.Projects.WorkpackageOnes.WorkpackageOne>(ProjectId);

        public WorkpackageOneController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult Overview()
        {
            return View();
        }

        public IActionResult Step(Guid id)
        {
            var step = WorkpackageOne.Steps.Single(x => x.Id == id);

            var model = new StepViewModel
            {
                Title = step.Title,
                StepId = id,
                Value = step.Value
            };

            return View(model);
        }

        public IActionResult EditStep(Guid id)
        {
            var step = WorkpackageOne.Steps.Single(x => x.Id == id);

            var model = new StepViewModel
            {
                Title = step.Title,
                StepId = id,
                Value = step.Value
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditStep(EditStepPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditStep(model.StepId);
            }

            var step = WorkpackageOne.Steps.Single(x => x.Id == model.StepId);

            ExecuteUserProjectCommand(new EditWorkpackageOneStepCommand
            {
                StepId = step.Id,
                NewValue = model.Value
            });

            if (!ModelState.IsValid)
            {
                return EditStep(model.StepId);
            }

            return RedirectToAction(nameof(Step), new { id = step.Id });
        }
    }
}
