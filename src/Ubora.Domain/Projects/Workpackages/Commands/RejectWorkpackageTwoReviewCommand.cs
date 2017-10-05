using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class RejectWorkpackageTwoReviewCommand : UserProjectCommand
    {
        public string ConcludingComment { get; set; }

        internal class Handler : CommandHandler<RejectWorkpackageTwoReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(RejectWorkpackageTwoReviewCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var canHandle = workpackageOne.DoesSatisfy(new CanRejectWorkpackageReview<WorkpackageTwo>());
                if (!canHandle)
                {
                    return CommandResult.Failed("Work package can not be rejected.");
                }

                var @event = new WorkpackageTwoReviewRejectedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    concludingComment: cmd.ConcludingComment,
                    rejectedAt: DateTimeOffset.Now);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
