using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class ReopenWorkpackageAfterAcceptanceByReviewCommand : UserProjectCommand
    {
        public Guid LatestReviewId { get; set; }

        internal class Handler : ICommandHandler<ReopenWorkpackageAfterAcceptanceByReviewCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(ReopenWorkpackageAfterAcceptanceByReviewCommand cmd)
            {
                _documentSession.LoadOrThrow<WorkpackageOne>(cmd.ProjectId);

                var @event = new WorkpackageOneReopenedAfterAcceptanceByReviewEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    acceptedReviewId: cmd.LatestReviewId);

                _documentSession.Events.Append(cmd.ProjectId, @event);
                _documentSession.SaveChanges();

                // TODO(Kaspar Kallas): Notify project leader (and members)

                return CommandResult.Success;
            }
        }
    }
}
