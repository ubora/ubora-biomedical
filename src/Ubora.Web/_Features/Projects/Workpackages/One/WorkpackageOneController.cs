using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Web._Features.Projects.Workpackages.One
{
    public class WorkpackageOneController : ProjectController
    {
        public WorkpackageOne WorkpackageOne => QueryProcessor.FindById<WorkpackageOne>(ProjectId);

        public WorkpackageOneController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult Version2Index()
        {
            return View();
        }

        public IActionResult Step(Guid id)
        {
            var step = WorkpackageOne.Steps.Single(x => x.Id == id);

            var model = new WorkpackageOneStepViewModel
            {
                Title = step.Title,
                StepId = id,
                Value = step.Value,
            };

            return View("Version2", model);
        }

        public IActionResult EditStep(Guid id)
        {
            var step = WorkpackageOne.Steps.Single(x => x.Id == id);

            var model = new WorkpackageOneStepViewModel
            {
                Title = step.Title,
                StepId = id,
                Value = step.Value
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditStep(WorkpackageOneStepViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditStep(model.StepId);
            }

            var step = WorkpackageOne.Steps.Single(x => x.Id == model.StepId);

            ExecuteUserProjectCommand(new EditWorkpackageOneStepCommand
            {
                StepId = step.Id,
                Title = step.Title,
                NewValue = model.Value
            });

            if (!ModelState.IsValid)
            {
                return EditStep(model.StepId);
            }

            return RedirectToAction(nameof(Step));
        }
    }
}
