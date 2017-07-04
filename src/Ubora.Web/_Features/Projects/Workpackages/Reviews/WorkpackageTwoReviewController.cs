using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;

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

        protected WorkpackageTwo WorkpackageTwo => FindById<WorkpackageTwo>(ProjectId);

        public async Task<IActionResult> Review()
        {
            var model = new WorkpackageReviewListViewModel
            {
                Reviews = WorkpackageTwo.Reviews.Select(_mapper.Map<WorkpackageReviewViewModel>),
                ReviewDecisionUrl = Url.Action(nameof(Decision)),
                SubmitForReviewUrl = Url.Action(nameof(SubmitForReview)),
                SubmitForReviewButton = await GetSubmitForReviewButtonVisibility()
            };

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

        private async Task<Visibility> GetSubmitForReviewButtonVisibility()
        {
            if (WorkpackageTwo.HasReviewInProcess || WorkpackageTwo.HasBeenAccepted)
            {
                return Visibility.CompletelyHidden();
            }
            else if (!await _authorizationService.AuthorizeAsync(User, Policies.CanSubmitWorkpackageForReview))
            {
                return Visibility.HiddenWithMessage("You can not submit work package for review, because you are not the project leader.");
            }
            else
            {
                return Visibility.Visible();
            }
        }
    }
}