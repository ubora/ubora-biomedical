using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageOne
{
    [ProjectRoute("WP1")]
    public class WorkpackageOneController : ProjectController
    {
        private readonly IMapper _mapper;

        public WorkpackageOneController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        protected Domain.Projects.Workpackages.WorkpackageOne WorkpackageOne => FindById<Domain.Projects.Workpackages.WorkpackageOne>(ProjectId);

        [Route("")]
        public IActionResult Overview()
        {
            return View();
        }

        [Route(nameof(DesignPlanning))]
        public IActionResult DesignPlanning()
        {
            var model = _mapper.Map<DesignPlanningViewModel>(Project);

            return View(model);
        }

        [Route(nameof(DesignPlanning))]
        [HttpPost]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult DesignPlanning(DesignPlanningViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return DesignPlanning();
            }

            ExecuteUserProjectCommand(new UpdateProjectCommand
            {
                AreaOfUsageTags = model.AreaOfUsageTags,
                ClinicalNeedTags = model.ClinicalNeedTags,
                PotentialTechnologyTags = model.PotentialTechnologyTags,
                Gmdn = model.Gmdn,
                Title = Project.Title
            });

            if (!ModelState.IsValid)
            {
                return DesignPlanning();
            }

            return View();
        }

        [Route(nameof(DeviceClassification))]
        public IActionResult DeviceClassification()
        {
            return View();
        }

        [Route("{stepId}")]
        public IActionResult Step(string stepId)
        {
            var step = WorkpackageOne.GetSingleStep(stepId);

            var model = _mapper.Map<StepViewModel>(step);

            return View(model);
        }

        // TODO: Hide in UI
        [Route("{stepId}/Edit")]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult EditStep(string stepId)
        {
            var step = WorkpackageOne.GetSingleStep(stepId);

            var model = _mapper.Map<StepViewModel>(step);

            return View(model);
        }

        [Route("{stepId}/Edit")]
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
    }
}
