using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageOneReviewController : ProjectController
    {
        private WorkpackageOne _workpackageOne;
        public WorkpackageOne WorkpackageOne => _workpackageOne ?? (_workpackageOne = QueryProcessor.FindById<WorkpackageOne>(ProjectId));

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData[nameof(PageTitle)] = "Workpackage one review";
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.Wp1MentorReview;
        }

        public virtual async Task<IActionResult> Review()
        {
            var latestReview = WorkpackageOne.GetLatestReviewOrNull();

            var project = Project;
            var model = new WorkpackageReviewListViewModel
            (
                reviews: WorkpackageOne.Reviews.OrderBy(r => r.SubmittedAt).Select(AutoMapper.Map<WorkpackageReviewViewModel>),
                latestReview: latestReview == null ? null : AutoMapper.Map<WorkpackageReviewViewModel>(latestReview),
                reviewDecisionUrl: Url.Action(nameof(Decision)),
                submitForReviewUrl: project.Members.Any(m => m.IsMentor) ? Url.Action(nameof(SubmitForReview)) : Url.Action(nameof(RequestMentoring)),
                submitForReviewButton: await WorkpackageReviewListViewModel.GetSubmitButtonVisibility(project, WorkpackageOne, User, AuthorizationService),
                requestMentoringButton: await WorkpackageReviewListViewModel.GetRequestMentoringButtonVisibility(project, WorkpackageOne, User, AuthorizationService)
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

            ExecuteUserProjectCommand(new SubmitWorkpackageOneForReviewCommand(), Notice.Success(SuccessTexts.WP1SubmittedForReview));

            if (!ModelState.IsValid)
            {
                return await Review();
            }

            return RedirectToAction(nameof(Review));
        }

        [HttpPost]
        [Authorize(Policies.CanRequestMentoring)]
        public async Task<IActionResult> RequestMentoring()
        {
            if (!ModelState.IsValid)
            {
                return await Review();
            }

            ExecuteUserProjectCommand(new RequestMentoringWorkpackageOneReviewCommand(), Notice.Success(SuccessTexts.WP1RequestedMentoring));

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
            }, Notice.Success(SuccessTexts.WP1ReviewAccepted));

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
            }, Notice.Success(SuccessTexts.WP1ReviewRejected));

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
            }, Notice.Success(SuccessTexts.WP1Reopened));

            if (!ModelState.IsValid)
            {
                return await Review();
            }

            return RedirectToAction(nameof(Review));
        }
    }
}
