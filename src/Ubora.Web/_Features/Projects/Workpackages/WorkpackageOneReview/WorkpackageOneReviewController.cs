using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.WorkpackageOnes;
using Ubora.Domain.Projects.WorkpackageSpecifications;
using Ubora.Domain.Projects.WorkpackageTwos;
using Ubora.Web.Authorization;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageOneReview
{
    public class WorkpackageReviewViewModel
    {
        public bool IsWorkpackageOneInReview { get; set; }
        public bool IsWorkpackageOneAccepted { get; set; }
    }

    public class WorkpackageReviewDecisionPostModel
    {
        public string Comment { get; set; }
    }

    // TODO
    public class WorkpackageOneReviewController : ProjectController
    {
        private readonly IMapper _mapper;

        public WorkpackageOneReviewController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        protected Domain.Projects.WorkpackageOnes.WorkpackageOne WorkpackageOne => this.FindById<Domain.Projects.WorkpackageOnes.WorkpackageOne>(ProjectId);

        [HttpPost]
        public IActionResult AssignMeAsMentor()
        {
            ExecuteUserProjectCommand(new AssignProjectMentorCommand
            {
                UserId = this.UserId
            });

            if (!ModelState.IsValid)
            {
                return Review();
            }

            return RedirectToAction(nameof(Review));
        }

        public IActionResult Review()
        {
            var isInReview= new HasReviewInStatus<Domain.Projects.WorkpackageOnes.WorkpackageOne>(WorkpackageReviewStatus.InReview);
            var isAccepted = new HasReviewInStatus<Domain.Projects.WorkpackageOnes.WorkpackageOne>(WorkpackageReviewStatus.Accepted);

            var model = new WorkpackageReviewViewModel
            {
                IsWorkpackageOneInReview = WorkpackageOne.DoesSatisfy(isInReview),
                IsWorkpackageOneAccepted = WorkpackageOne.DoesSatisfy(isAccepted)
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

            ExecuteUserProjectCommand(new SubmitWorkpackageOneForReviewCommand());

            if (!ModelState.IsValid)
            {
                return Review();
            }

            return RedirectToAction(nameof(Review));
        }

        [Authorize(Policies.CanReviewProjectWorkpackages)]
        public IActionResult Decision()
        {
            // todo
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

            ExecuteUserProjectCommand(new AcceptWorkpackageOneByReviewCommand());

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

            ExecuteUserProjectCommand(new RejectWorkpackageOneByReviewCommand());

            if (!ModelState.IsValid)
            {
                return Review();
            }

            return RedirectToAction(nameof(Review));
        }
    }
}
