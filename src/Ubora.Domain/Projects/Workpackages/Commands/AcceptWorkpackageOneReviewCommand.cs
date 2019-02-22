using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    /// <summary>
    /// Used by mentors to accept project's in review workpackage draft. Opens up the next workpackage.
    /// </summary>
    public class AcceptWorkpackageOneReviewCommand : UserProjectCommand
    {
        public string ConcludingComment { get; set; }

        internal class Handler : CommandHandler<AcceptWorkpackageOneReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AcceptWorkpackageOneReviewCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageOne>(cmd.ProjectId);

                var canHandle = workpackageOne.DoesSatisfy(new CanWorkpackageBeAcceptedByReview<WorkpackageOne>());
                if (!canHandle)
                {
                    return CommandResult.Failed("Work package can not be accepted.");
                }

                var @event = new WorkpackageOneReviewAcceptedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    concludingComment: cmd.ConcludingComment,
                    acceptedAt: DateTimeOffset.Now,
                    deviceStructuredInformationId: cmd.ProjectId);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}