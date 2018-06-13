using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class RequestMentoringWorkpackageOneReviewCommand : UserProjectCommand
    {
        internal class Handler : CommandHandler<RequestMentoringWorkpackageOneReviewCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(RequestMentoringWorkpackageOneReviewCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageOne>(cmd.ProjectId);



                if (workpackageOne.HasBeenRequestedMentoring == true)
                {
                    return CommandResult.Failed("Not allow again request mentoring!");
                }

                var @event = new WorkpackageOneReviewRequestedMentoringEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
