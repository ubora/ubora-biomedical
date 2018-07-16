using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class UnlockWorkpackageCommand : UserProjectCommand
    {
        public WorkpackageType WorkpackageType { get; set; } 
        
        internal class Handler : CommandHandler<UnlockWorkpackageCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(UnlockWorkpackageCommand cmd)
            {                
                switch (cmd.WorkpackageType)
                {
                    case WorkpackageType.Three:
                        var workpackageThree = DocumentSession.LoadOrThrow<WorkpackageThree>(cmd.ProjectId);
                        var workpackageThreeUnlockedEvent = new WorkpackageThreeUnlockedEvent(initiatedBy: cmd.Actor, projectId: cmd.ProjectId);
                        DocumentSession.Events.Append(cmd.ProjectId, workpackageThreeUnlockedEvent);
                        break;
                    
                    case WorkpackageType.Four:
                        var workpackageFour = DocumentSession.LoadOrThrow<WorkpackageFour>(cmd.ProjectId);
                        var workpackageFourUnlockedEvent = new WorkpackageFourUnlockedEvent(initiatedBy: cmd.Actor, projectId: cmd.ProjectId);
                        DocumentSession.Events.Append(cmd.ProjectId, workpackageFourUnlockedEvent);
                        break;
                }
                
                DocumentSession.SaveChanges();
                return CommandResult.Success;
            }
        }
    }
}