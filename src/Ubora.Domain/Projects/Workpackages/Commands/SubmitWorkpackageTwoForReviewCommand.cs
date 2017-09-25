using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class SubmitWorkpackageTwoForReviewCommand : UserProjectCommand
    {
        internal class Handler : CommandHandler<SubmitWorkpackageTwoForReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(SubmitWorkpackageTwoForReviewCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var canHandle = workpackageOne.DoesSatisfy(new CanSubmitWorkpackageReview<WorkpackageTwo>());
                if (!canHandle)
                {
                    return CommandResult.Failed("Work package can not be submitted for review.");
                }

                var @event = new WorkpackageTwoSubmittedForReviewEvent(
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