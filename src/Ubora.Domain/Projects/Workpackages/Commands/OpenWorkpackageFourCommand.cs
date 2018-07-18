using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class OpenWorkpackageFourCommand : UserProjectCommand
    {
        internal class Handler : CommandHandler<OpenWorkpackageFourCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
                
            }

            public override ICommandResult Handle(OpenWorkpackageFourCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageOne>(cmd.ProjectId);
                if (!workpackageOne.HasBeenAccepted)
                {
                    return CommandResult.Failed("Work package one hasn't been accepted.");
                }
                    
                var workpackageTwo = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var workPackageThree = DocumentSession.Load<WorkpackageThree>(cmd.ProjectId);
                if (!workPackageThree.HasBeenOpened)
                {
                    return CommandResult.Failed("Work package three hasn't been opened.");
                }
                
                var workPackageFour = DocumentSession.Load<WorkpackageFour>(cmd.ProjectId);
                if (workPackageFour != null)
                {
                    return CommandResult.Failed("Work package is already opened.");
                }

                var @event = new WorkpackageFourOpenedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId
                );
                
                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();
                
                return CommandResult.Success;
            }
        }
    }
}