using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageOneReviewController : ProjectController
    {
        private WorkpackageOne _workpackageOne;
        public WorkpackageOne WorkpackageOne => _workpackageOne ?? (_workpackageOne = QueryProcessor.FindById<WorkpackageOne>(ProjectId));

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Title"] = "Workpackage one review";
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.Wp1MentorReview;
        }

        public virtual async Task<IActionResult> Review()
        {
            var latestReview = WorkpackageOne.GetLatestReviewOrNull();

            var model = new WorkpackageReviewListViewModel
            (
                reviews: WorkpackageOne.Reviews.OrderBy(r => r.SubmittedAt).Select(AutoMapper.Map<WorkpackageReviewViewModel>),
                latestReview: latestReview == null ? null : AutoMapper.Map<WorkpackageReviewViewModel>(latestReview),
                reviewDecisionUrl: Url.Action(nameof(Decision)),
                submitForReviewUrl: Url.Action(nameof(SubmitForReview)),
                submitForReviewButton: await WorkpackageReviewListViewModel.GetSubmitButtonVisibility(WorkpackageOne, User, AuthorizationService)
            );

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

            ExecuteUserProjectCommand(new SubmitWorkpackageOneForReviewCommand());

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

            ExecuteUserProjectCommand(new AcceptWorkpackageOneReviewCommand
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

            ExecuteUserProjectCommand(new RejectWorkpackageOneReviewCommand
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
        public async Task<IActionResult> ReopenWorkpackageAfterAcceptance(ReopenWorkpackageAfterAcceptanceByReviewPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Review();
            }

            ExecuteUserProjectCommand(new ReopenWorkpackageAfterAcceptanceByReviewCommand
            {
                LatestReviewId = model.LatestReviewId 
            });

            if (!ModelState.IsValid)
            {
                return await Review();
            }

            return RedirectToAction(nameof(Review));
        }
    }
}
