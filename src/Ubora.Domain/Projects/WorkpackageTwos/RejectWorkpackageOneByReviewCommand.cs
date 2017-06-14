using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.WorkpackageOnes;
using Ubora.Domain.Projects.WorkpackageSpecifications;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    public class RejectWorkpackageOneByReviewCommand : UserProjectCommand
    {
        public string Conclusion { get; set; }

        internal class Handler : CommandHandler<RejectWorkpackageOneByReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(RejectWorkpackageOneByReviewCommand cmd)
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

                var activeReview = workpackageOne.Reviews
                    .SingleOrDefault(x => x.Status == WorkpackageReviewStatus.InReview);
                // TODO:

                var @event = new WorkpackageOneRejectedByReviewEvent(
                    initiatedBy: cmd.Actor,
                    workpackageOneId: cmd.ProjectId,
                    reviewId: activeReview.Id);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}