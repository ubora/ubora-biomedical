using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Web.Authorization;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageOneReview
{
    public class WorkpackageReviewViewModel
    {
        public WorkpackageReviewStatus Status { get; set; }
        public bool InReview { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ConcludedAt { get; set; }
    }

    public class WorkpackageReviewDecisionPostModel
    {
        public bool IsAccept { get; set; }
        public bool IsReject { get; set; }
        public string ConcludingComment { get; set; }
    }

    // TODO
    public class WorkpackageOneReviewController : ProjectController
    {
        private readonly IMapper _mapper;

        public WorkpackageOneReviewController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        protected Domain.Projects.Workpackages.WorkpackageOne WorkpackageOne => this.FindById<Domain.Projects.Workpackages.WorkpackageOne>(ProjectId);

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
            //var isInReview= new HasReviewInStatus<Domain.Projects.WorkpackageOnes.WorkpackageOne>(WorkpackageReviewStatus.InReview);
            //var isAccepted = new HasReviewInStatus<Domain.Projects.WorkpackageOnes.WorkpackageOne>(WorkpackageReviewStatus.Accepted);

            var reviews = WorkpackageOne.Reviews
                .Select(x => new WorkpackageReviewViewModel
                {
                    Status = x.Status,
                    InReview = x.Status == WorkpackageReviewStatus.InReview,
                    IsAccepted = x.Status == WorkpackageReviewStatus.Accepted,
                    IsRejected = x.Status == WorkpackageReviewStatus.Rejected,
                    Comment = x.ConcludingComment,
                    CreatedAt = x.CreatedAt,
                    ConcludedAt= x.ConcludedAt
                });

            //var model = new WorkpackageReviewViewModel
            //{
            //    IsWorkpackageOneInReview = WorkpackageOne.DoesSatisfy(isInReview),
            //    IsWorkpackageOneAccepted = WorkpackageOne.DoesSatisfy(isAccepted)
            //};

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

            ExecuteUserProjectCommand(new SubmitWorkpackageOneForReviewCommand());

            if (!ModelState.IsValid)
            {
                return Review();
            }

            return RedirectToAction(nameof(Review));
        }

        [Authorize(Policies.CanReviewProjectWorkpackages)]
        public IActionResult Decision(WorkpackageReviewDecisionPostModel model)
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

            ExecuteUserProjectCommand(new AcceptWorkpackageOneReviewCommand
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

            ExecuteUserProjectCommand(new RejectWorkpackageOneReviewCommand
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
