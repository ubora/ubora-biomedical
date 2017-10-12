using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class AcceptWorkpackageTwoReviewCommand : UserProjectCommand
    {
        public string ConcludingComment { get; set; }

        internal class Handler : CommandHandler<AcceptWorkpackageTwoReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AcceptWorkpackageTwoReviewCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var canHandle = workpackageOne.DoesSatisfy(new CanWorkpackageBeAcceptedByReview<WorkpackageTwo>());
                if (!canHandle)
                {
                    return CommandResult.Failed("Work package can not be accepted.");
                }

                var @event = new WorkpackageTwoReviewAcceptedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    concludingComment: cmd.ConcludingComment,
                    acceptedAt: DateTimeOffset.Now);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
