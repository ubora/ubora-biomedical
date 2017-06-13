using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageOnes;
using Ubora.Domain.Projects.WorkpackageTwos;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageOneReview
{
    public class ProjectReviewViewModel
    {
        public bool IsWorkpackageOneInReview { get; set; }
        public bool IsWorkpackageOneAccepted { get; set; }
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

        public IActionResult Review()
        {
            var isInReview= new HasReviewInStatus<Domain.Projects.WorkpackageOnes.WorkpackageOne>(WorkpackageReviewStatus.InReview);
            var isAccepted = new HasReviewInStatus<Domain.Projects.WorkpackageOnes.WorkpackageOne>(WorkpackageReviewStatus.Accepted);

            var model = new ProjectReviewViewModel
            {
                IsWorkpackageOneInReview = WorkpackageOne.DoesSatisfy(isInReview),
                IsWorkpackageOneAccepted = WorkpackageOne.DoesSatisfy(isAccepted)
            };

            return View(nameof(Review), model);
        }

        [HttpPost]
        public IActionResult SubmitForReview()
        {
            // TODO: Authorize

            // TODO: Validate?

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

        public IActionResult Decision()
        {
            return null;
        }

        [HttpPost]
        public IActionResult Accept()
        {
            // TODO: Authorize

            // TODO: Validate?

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
        public IActionResult Reject()
        {
            // TODO: Authorize

            // TODO: Validate?

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
