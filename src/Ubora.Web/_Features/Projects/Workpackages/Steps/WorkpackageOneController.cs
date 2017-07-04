using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP1")]
    public class WorkpackageOneController : ProjectController
    {
        private readonly IMapper _mapper;

        public WorkpackageOneController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        private WorkpackageOne _workpackageOne;
        public WorkpackageOne WorkpackageOne
        {
            get => _workpackageOne ?? (_workpackageOne = FindById<WorkpackageOne>(ProjectId));
            private set => _workpackageOne = value;
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
        public IActionResult Read(string stepId)
        {
            var step = WorkpackageOne.GetSingleStep(stepId);
            var model = _mapper.Map<ReadStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });
            model.EditButton = GetEditButtonVisibility();

            UiElementVisibility GetEditButtonVisibility()
            {
                if (WorkpackageOne.HasReviewInProcess || WorkpackageOne.HasBeenAccepted)
                {
                    return UiElementVisibility.HiddenWithMessage("You can not edit work package when it's under review or has been accepted by review.");
                }
                return UiElementVisibility.Visible();
            }

            return View(model);
        }

        [Route("{stepId}/Edit")]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult Edit(string stepId)
        {
            var step = WorkpackageOne.GetSingleStep(stepId);

            var model = _mapper.Map<EditStepViewModel>(step);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });

            return View(model);
        }

        [Route("{stepId}/Edit")]
        [HttpPost]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult Edit(EditStepPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Edit(model.StepId);
            }

            ExecuteUserProjectCommand(new EditWorkpackageOneStepCommand
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
