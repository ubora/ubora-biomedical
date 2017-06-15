using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class RejectWorkpackageOneReviewCommand : UserProjectCommand
    {
        public string ConcludingComment { get; set; }

        internal class Handler : CommandHandler<RejectWorkpackageOneReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(RejectWorkpackageOneReviewCommand cmd)
            {
                var workpackageOne = DocumentSession.Load<WorkpackageOne>(cmd.ProjectId);
                if (workpackageOne == null)
                {
                    throw new InvalidOperationException($"{nameof(WorkpackageOne)} not found with id [{cmd.ProjectId}]");
                }

                var canHandle = workpackageOne.DoesSatisfy(new CanWorkpackageBeRejectedByReview<WorkpackageOne>());
                if (!canHandle)
                {
                    return new CommandResult("Work package can not be rejected.");
                }

                var @event = new WorkpackageOneRejectedByReviewEvent(
                    initiatedBy: cmd.Actor,
                    workpackageOneId: cmd.ProjectId,
                    concludingComment: cmd.ConcludingComment);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}