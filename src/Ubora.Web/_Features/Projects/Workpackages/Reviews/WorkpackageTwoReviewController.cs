using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageTwoReviewController : ProjectController
    {
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public WorkpackageTwoReviewController(ICommandQueryProcessor processor, IMapper mapper, IAuthorizationService authorizationService) : base(processor)
        {
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        private WorkpackageTwo _workpackageTwo;
        public WorkpackageTwo WorkpackageTwo
        {
            get => _workpackageTwo ?? (_workpackageTwo = FindById<WorkpackageTwo>(ProjectId));
            private set => _workpackageTwo = value;
        }

        public async Task<IActionResult> Review()
        {
            var model = new WorkpackageReviewListViewModel
            {
                Reviews = WorkpackageTwo.Reviews.Select(_mapper.Map<WorkpackageReviewViewModel>),
                ReviewDecisionUrl = Url.Action(nameof(Decision)),
                SubmitForReviewUrl = Url.Action(nameof(SubmitForReview)),
                SubmitForReviewButton = await GetSubmitButtonVisibility()
            };

            async Task<UiElementVisibility> GetSubmitButtonVisibility()
            {
                if (WorkpackageTwo.HasReviewInProcess || WorkpackageTwo.HasBeenAccepted)
                {
                    return UiElementVisibility.HiddenCompletely();
                }
                if (!await _authorizationService.AuthorizeAsync(User, Policies.CanSubmitWorkpackageForReview))
                {
                    return UiElementVisibility.HiddenWithMessage("You can not submit work package for review, because you are not a project leader.");
                }
                return UiElementVisibility.Visible();
            }

            return View(nameof(Review), model);
        }

        [HttpPost]
        [Authorize(Policies.CanSubmitWorkpackageForReview)]
        public async Task<IActionResult> SubmitForReview()
        {
            if (!ModelState.IsValid)
            {
                return await Review();
            }

            ExecuteUserProjectCommand(new SubmitWorkpackageTwoForReviewCommand());

            if (!ModelState.IsValid)
            {
                return await Review();
            }

            return RedirectToAction(nameof(Review));
        }

        [Authorize(Policies.CanReviewProjectWorkpackages)]
        public IActionResult Decision()
        {
            var model = new WorkpackageReviewDecisionPostModel
            {
                AcceptUrl = Url.Action(nameof(Accept)),
                RejectUrl = Url.Action(nameof(Reject))
            };
            return View(nameof(Decision), model);
        }

        [HttpPost]
        [Authorize(Policies.CanReviewProjectWorkpackages)]
        public async Task<IActionResult> Accept(WorkpackageReviewDecisionPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Review();
            }

            ExecuteUserProjectCommand(new AcceptWorkpackageTwoReviewCommand
            {
                ConcludingComment = model.ConcludingComment
            });

            if (!ModelState.IsValid)
            {
                return await Review();
            }

            return RedirectToAction(nameof(Review));
        }

        [HttpPost]
        [Authorize(Policies.CanReviewProjectWorkpackages)]
        public async Task<IActionResult> Reject(WorkpackageReviewDecisionPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Review();
            }

            ExecuteUserProjectCommand(new RejectWorkpackageTwoReviewCommand
            {
                ConcludingComment = model.ConcludingComment
            });

            if (!ModelState.IsValid)
            {
                return await Review();
            }

            return RedirectToAction(nameof(Review));
        }
    }
}