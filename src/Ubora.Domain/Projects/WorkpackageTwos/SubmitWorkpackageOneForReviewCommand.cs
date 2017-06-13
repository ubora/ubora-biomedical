using System;
using System.Linq.Expressions;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.WorkpackageOnes;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    //public class WorkpackageOneLockedEvent : UboraEvent
    //{
    //    public WorkpackageOneLockedEvent(UserInfo initiatedBy) : base(initiatedBy)
    //    {
    //    }

    //    public override string GetDescription() => "Locked work package.";
    //}

    //public class WorkpackageOneUnlockedEvent : UboraEvent
    //{
    //    public WorkpackageOneUnlockedEvent(UserInfo initiatedBy) : base(initiatedBy)
    //    {
    //    }

    //    public override string GetDescription() => "Unlocked work package.";
    //}

    public class CanBeSubmittedForReview<TWorkpackage> : WrappedSpecification<TWorkpackage> where TWorkpackage : Workpackage<TWorkpackage>
    {
        public override Specification<TWorkpackage> ToSpecification()
        {
            var isAlreadyAccepted = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.Accepted);
            var isAlreadyInReview = new HasReviewInStatus<TWorkpackage>(WorkpackageReviewStatus.InReview);

            return !(isAlreadyAccepted || isAlreadyInReview);
        }
    }

    /// <summary>
    /// Submit workpackage for formal review. Workpackage will be locked for new edits while in review.
    /// </summary>
    public class SubmitWorkpackageOneForReviewCommand : UserProjectCommand
    {
        internal class Handler : CommandHandler<SubmitWorkpackageOneForReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(SubmitWorkpackageOneForReviewCommand cmd)
            {
                // lock if workpackage one

                var workpackageOne = DocumentSession.Load<WorkpackageOne>(cmd.ProjectId);
                if (workpackageOne == null)
                {
                    throw new InvalidOperationException($"{nameof(WorkpackageOne)} not found with id [{cmd.ProjectId}]");
                }

                var canHandle = workpackageOne.DoesSatisfy(new CanBeSubmittedForReview<WorkpackageOne>());
                if (!canHandle)
                {
                    return new CommandResult("Work package can not be submitted for review.");
                }

                var @event = new WorkpackageOneSubmittedForReviewEvent(cmd.Actor, cmd.ProjectId);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}