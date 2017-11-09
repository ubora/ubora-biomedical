using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Ubora.Domain.Projects.Candidates.Specifications;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP2")]
    public class WorkpackageTwoController : ProjectController
    {
        private WorkpackageTwo _workpackageTwo;
        private readonly CandidateItemViewModel.Factory _candidateItemViewModelFactory;

        public WorkpackageTwo WorkpackageTwo => _workpackageTwo ?? (_workpackageTwo = QueryProcessor.FindById<WorkpackageTwo>(ProjectId));

        public WorkpackageTwoController(CandidateItemViewModel.Factory candidateItemViewModelFactory)
        {
            _candidateItemViewModelFactory = candidateItemViewModelFactory;
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

        [Route(nameof(Voting))]
        public IActionResult Voting()
        {
            var candidates = QueryProcessor.Find(new IsProjectCandidateSpec(ProjectId));

            var candidateViewModels = candidates.Select(candidate => _candidateItemViewModelFactory.Create(candidate));
            var model = new VotingViewModel
            {
                Candidates = candidateViewModels
            };

            return View(nameof(Voting), model);
        }
    }
}
