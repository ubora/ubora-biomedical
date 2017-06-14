using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.WorkpackageOnes;
using Ubora.Domain.Projects.WorkpackageSpecifications;

namespace Ubora.Domain.Projects.WorkpackageTwos
{
    /// <summary>
    /// Used by mentors to accept project's in review workpackage draft. Opens up the next workpackage.
    /// </summary>
    public class AcceptWorkpackageOneByReviewCommand : UserProjectCommand
    {
        public string Conclusion { get; set; }

        internal class Handler : CommandHandler<AcceptWorkpackageOneByReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AcceptWorkpackageOneByReviewCommand cmd)
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

                var activeReview = workpackageOne.Reviews
                    .SingleOrDefault(x => x.Status == WorkpackageReviewStatus.InReview);

                var @event = new WorkpackageOneAcceptedByReviewEvent(
                    initiatedBy: cmd.Actor,
                    workpackageOneId: cmd.ProjectId,
                    reviewId: activeReview.Id);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                //var workpackageTwo = DocumentSession.Load<WorkpackageTwo>(cmd.ProjectId);
                //if (workpackageTwo != null)
                //{
                    // todo
                //}

                return new CommandResult();
            }
        }
    }
}