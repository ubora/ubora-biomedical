using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
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
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageOne>(cmd.ProjectId);

                var canHandle = workpackageOne.DoesSatisfy(new CanSubmitWorkpackageReview<WorkpackageOne>());
                if (!canHandle)
                {
                    return CommandResult.Failed("Work package can not be submitted for review.");
                }

                var @event = new WorkpackageOneSubmittedForReviewEvent(
                    initiatedBy: cmd.Actor, 
                    projectId: cmd.ProjectId,
                    reviewId: Guid.NewGuid(),
                    submittedAt: DateTimeOffset.Now);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}