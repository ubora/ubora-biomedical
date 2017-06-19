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
                var workpackageOne = DocumentSession.Load<WorkpackageOne>(cmd.ProjectId);
                if (workpackageOne == null)
                {
                    throw new InvalidOperationException($"{nameof(WorkpackageOne)} not found with id [{cmd.ProjectId}]");
                }

                var canHandle = workpackageOne.DoesSatisfy(new CanWorkpackageBeAcceptedByReview<WorkpackageOne>());
                if (!canHandle)
                {
                    return new CommandResult("Work package can not be accepted.");
                }

                var @event = new WorkpackageOneAcceptedByReviewEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    concludingComment: cmd.ConcludingComment,
                    acceptedAt: DateTimeOffset.Now);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}