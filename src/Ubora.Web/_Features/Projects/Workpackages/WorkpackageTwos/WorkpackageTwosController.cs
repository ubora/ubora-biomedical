using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOne;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageTwos
{
    [ProjectRoute("WP2")]
    public class WorkpackageTwosController : ProjectController
    {
        private readonly IMapper _mapper;

        public WorkpackageTwosController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        protected WorkpackageTwo WorkpackageTwo => FindById<WorkpackageTwo>(ProjectId);

        public IActionResult Index()
        {
            return View();
        }

        [Route("{stepId}")]
        public IActionResult Read(string stepId)
        {
            var step = WorkpackageTwo.GetSingleStep(stepId);

            var model = _mapper.Map<StepViewModel>(step);

            return View(model);
        }

        [Route("{stepId}/Edit")]
        public IActionResult Edit(string stepId)
        {
            var step = WorkpackageTwo.GetSingleStep(stepId);

            var model = _mapper.Map<StepViewModel>(step);

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
