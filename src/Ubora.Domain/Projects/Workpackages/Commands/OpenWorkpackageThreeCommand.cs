using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class OpenWorkpackageThreeCommand : UserProjectCommand
    {
        internal class Handler : CommandHandler<OpenWorkpackageThreeCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(OpenWorkpackageThreeCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageOne>(cmd.ProjectId);
                var workpackageTwo = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var workPackageThree = DocumentSession.Load<WorkpackageThree>(cmd.ProjectId);
                if (workPackageThree != null)
                {
                    return CommandResult.Failed("Work package is already opened.");
                }

                var @event = new WorkpackageThreeOpenedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
