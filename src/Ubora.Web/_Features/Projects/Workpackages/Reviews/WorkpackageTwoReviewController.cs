using System.Linq;
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

        public WorkpackageTwoReviewController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
            {
            _mapper = mapper;
        }

        protected WorkpackageTwo WorkpackageTwo => FindById<WorkpackageTwo>(ProjectId);

        public IActionResult Review()
        {
            var model = new WorkpackageReviewListViewModel
            {
                Reviews = WorkpackageTwo.Reviews.Select(_mapper.Map<WorkpackageReviewViewModel>),
                ReviewDecisionUrl = Url.Action(nameof(Decision)),
                SubmitForReviewUrl = Url.Action(nameof(SubmitForReview))
            };

            return View(nameof(Review), model);
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