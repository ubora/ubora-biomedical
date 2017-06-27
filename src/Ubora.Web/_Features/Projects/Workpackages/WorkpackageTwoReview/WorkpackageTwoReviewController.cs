using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOneReview;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageTwoReview
{
    public class WorkpackageTwoReviewController : ProjectController
    {
        private readonly IMapper _mapper;

        public WorkpackageTwoReviewController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
            {
            _mapper = mapper;
        }

        protected WorkpackageTwo WorkpackageTwo => FindById<WorkpackageTwo>(ProjectId);

        public IActionResult Review()
        {
            var reviews = WorkpackageTwo.Reviews
                .Select(_mapper.Map<WorkpackageReviewViewModel>);

            return View(nameof(Review), reviews);
        }

        [HttpPost]
        [Authorize(Policies.CanSubmitWorkpackageForReview)]
        public IActionResult SubmitForReview()
        {
            if (!ModelState.IsValid)
            {
                return Review();
            }

            ExecuteUserProjectCommand(new SubmitWorkpackageTwoForReviewCommand());

            if (!ModelState.IsValid)
            {
                return Review();
            }

            return RedirectToAction(nameof(Review));
        }

        [Authorize(Policies.CanReviewProjectWorkpackages)]
        public IActionResult Decision(WorkpackageReviewDecisionPostModel model)
        {
            return View(nameof(Decision));
        }

        [HttpPost]
        [Authorize(Policies.CanReviewProjectWorkpackages)]
        public IActionResult Accept(WorkpackageReviewDecisionPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Review();
            }

            ExecuteUserProjectCommand(new AcceptWorkpackageTwoReviewCommand
            {
                ConcludingComment = model.ConcludingComment
            });

            if (!ModelState.IsValid)
            {
                return Review();
            }

            return RedirectToAction(nameof(Review));
        }

        [HttpPost]
        [Authorize(Policies.CanReviewProjectWorkpackages)]
        public IActionResult Reject(WorkpackageReviewDecisionPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return Review();
            }

            ExecuteUserProjectCommand(new RejectWorkpackageTwoReviewCommand
            {
                ConcludingComment = model.ConcludingComment
            });

            if (!ModelState.IsValid)
            {
                return Review();
            }

            return RedirectToAction(nameof(Review));
        }
    }
}