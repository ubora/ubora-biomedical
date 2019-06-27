using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects.BusinessModelCanvases;
using Ubora.Domain.Projects.BusinessModelCanvases.Command;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    [ProjectRoute("WP5")]
    [WorkpackageStepIdFromRouteToViewData]
    public class WorkpackageFiveController : ProjectController
    {
        public const string Name = "WorkpackageFive";

        private WorkpackageFive _workpackageFive;
        public WorkpackageFive WorkpackageFive =>
            _workpackageFive ?? (_workpackageFive = QueryProcessor.FindById<WorkpackageFive>(ProjectId));

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData[nameof(ProjectMenuOption)] = ProjectMenuOption.Workpackages;
            ViewData[nameof(PageTitle)] = "WP 5: Operation";
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (WorkpackageFive == null)
                return RedirectToAction(nameof(Unlocking));
            return RedirectToAction(nameof(Read), new { stepId = WorkpackageFive.Steps.First().Id });
        }

        [HttpGet("unlock")]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkpackages))]
        public IActionResult Unlocking()
        {
            if (WorkpackageFive != null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData[nameof(PageTitle)] = "WP 5: Operation";
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.WorkpackageFiveLocked;

            return View("UnlockWp5");
        }

        [HttpPost("unlock")]
        [Authorize(Policy = nameof(Policies.CanUnlockWorkpackages))]
        public IActionResult Unlock()
        {
            ExecuteUserProjectCommand(new OpenWorkpackageFiveCommand(), Notice.Success(SuccessTexts.WPUnlocked));

            if (!ModelState.IsValid)
            {
                return Unlocking();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{stepId}")]
        public async Task<IActionResult> Read(string stepId)
        {
            var step = WorkpackageFive.GetSingleStep(stepId);
            var model = AutoMapper.Map<ReadStepViewModel>(step);
            model.ContentHtml = await ConvertQuillDeltaToHtml(step.ContentV2);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });
            model.EditButton = UiElementVisibility.Visible();

            return View(model);
        }

        [HttpGet("{stepId}/Edit")]
        public async Task<IActionResult> Edit(string stepId)
        {
            var step = WorkpackageFive.GetSingleStep(stepId);
            var model = AutoMapper.Map<EditStepViewModel>(step);
            model.ContentQuillDelta = await SanitizeQuillDeltaForEditing(step.ContentV2);
            model.EditStepUrl = Url.Action(nameof(Edit), new { stepId });
            model.ReadStepUrl = Url.Action(nameof(Read), new { stepId });

            return View(model);
        }

        [HttpPost("{stepId}/Edit")]
        public async Task<IActionResult> Edit(EditStepPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Edit(model.StepId);
            }

            ExecuteUserProjectCommand(new EditWorkpackageFiveStepCommand
            {
                StepId = model.StepId,
                NewValue = new QuillDelta(model.ContentQuillDelta)
            }, Notice.Success(SuccessTexts.WP5StepEdited));

            if (!ModelState.IsValid)
            {
                return await Edit(model.StepId);
            }

            return RedirectToAction(nameof(Read), new { stepId = model.StepId });
        }

        [HttpGet("BusinessModelCanvas")]
        public async Task<IActionResult> BusinessModelCanvas()
        {
            var businessModelCanvas = QueryProcessor.FindById<BusinessModelCanvas>(ProjectId);
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.BusinessModelCanvas;
            return View(new BusinessModelCanvasReadViewModel
            {
                ValueProposalDescriptionHtml = await ConvertQuillDeltaToHtml(businessModelCanvas.ValueProposalDescription),
                GrowthStrategyDescriptionHtml = await ConvertQuillDeltaToHtml(businessModelCanvas.GrowthStrategyDescription),
                KeyResourcesAndPartnersDescriptionHtml = await ConvertQuillDeltaToHtml(businessModelCanvas.KeyResourcesAndPartnersDescription),
                PotentialClientsAndUsersAndChannelsDescriptionHtml = await ConvertQuillDeltaToHtml(businessModelCanvas.PotentialClientsAndUsersAndChannelsDescription),
                RelevantDocumentationForProductionAndUseDescriptionHtml = await ConvertQuillDeltaToHtml(businessModelCanvas.RelevantDocumentationForProductionAndUseDescription),
                AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescriptionHtml = await ConvertQuillDeltaToHtml(businessModelCanvas.AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription)
            });
        }

        [HttpGet("BusinessModelCanvas/Edit")]
        public IActionResult EditBusinessModelCanvas()
        {
            var businessModelCanvas = QueryProcessor.FindById<BusinessModelCanvas>(ProjectId);

            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.BusinessModelCanvas;
            return View("BusinessModelCanvasEdit", new BusinessModelCanvasEditPostModel
            {
                ValueProposalDescriptionQuillDelta = businessModelCanvas.ValueProposalDescription.Value,
                GrowthStrategyDescriptionQuillDelta = businessModelCanvas.GrowthStrategyDescription.Value,
                KeyResourcesAndPartnersDescriptionQuillDelta = businessModelCanvas.KeyResourcesAndPartnersDescription.Value,
                PotentialClientsAndUsersAndChannelsDescriptionQuillDelta = businessModelCanvas.PotentialClientsAndUsersAndChannelsDescription.Value,
                RelevantDocumentationForProductionAndUseDescriptionQuillDelta = businessModelCanvas.RelevantDocumentationForProductionAndUseDescription.Value,
                AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescriptionQuillDelta = businessModelCanvas.AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription.Value
            });
        }

        [HttpPost("BusinessModelCanvas/Edit")]
        public IActionResult EditBusinessModelCanvas(BusinessModelCanvasEditPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditBusinessModelCanvas();
            }

            ExecuteUserProjectCommand(new EditBusinessModelCanvasCommand
            {
                ValueProposalDescription = new QuillDelta(model.ValueProposalDescriptionQuillDelta),
                GrowthStrategyDescription = new QuillDelta(model.GrowthStrategyDescriptionQuillDelta),
                KeyResourcesAndPartnersDescription = new QuillDelta(model.KeyResourcesAndPartnersDescriptionQuillDelta),
                PotentialClientsAndUsersAndChannelsDescription = new QuillDelta(model.PotentialClientsAndUsersAndChannelsDescriptionQuillDelta),
                RelevantDocumentationForProductionAndUseDescription = new QuillDelta(model.RelevantDocumentationForProductionAndUseDescriptionQuillDelta),
                AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription = new QuillDelta(model.AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescriptionQuillDelta),
            }, Notice.Success(SuccessTexts.WP5BusinessModelCanvasEdited));

            if (!ModelState.IsValid)
            {
                return EditBusinessModelCanvas();
            }

            return RedirectToAction(nameof(BusinessModelCanvas));
        }

        [HttpGet("AgreeToTermsOfUbora")]
        public IActionResult AgreeToTermsOfUbora()
        {
            var model = new AgreeToTermsOfUboraViewModel
            {
                IsAgreed = Project.IsAgreedToTermsOfUbora
            };
            
            return View("AgreeToTermsOfUbora", model);
        }

        [HttpPost("AgreeToTermsOfUbora")]
        public IActionResult AgreeToTermsOfUbora(AgreeToTermsOfUboraPostModel model)
        {
            if (!AuthorizationService.IsAuthorized(User, Policies.CanChangeAgreementToTermsOfUbora))
            {
                return Forbid();
            }
            
            if (!ModelState.IsValid)
            {
                return AgreeToTermsOfUbora();
            }

            ExecuteUserProjectCommand(new ChangeAgreementToTermsOfUboraCommand
            {
                IsAgreed = model.IsAgreed
            }, Notice.Success(SuccessTexts.WP5AgreementToTermsOfUboraChanged));

            return RedirectToAction(nameof(AgreeToTermsOfUbora));
        }
    }
}
