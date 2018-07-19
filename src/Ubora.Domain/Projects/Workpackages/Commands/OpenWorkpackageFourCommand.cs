using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class OpenWorkpackageFourCommand : UserProjectCommand
    {
        public Guid DeviceStructuredInformationId { get; set; }
        
        internal class Handler : CommandHandler<OpenWorkpackageFourCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(OpenWorkpackageFourCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageOne>(cmd.ProjectId);
                var workpackageTwo = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);
 
                var workPackageFour = DocumentSession.Load<WorkpackageFour>(cmd.ProjectId);
                if (workPackageFour != null)
                {
                    return CommandResult.Failed("Work package is already opened.");
                }
                           
                var @event = new WorkpackageFourOpenedEvent(
                    deviceStructuredInformationId: cmd.DeviceStructuredInformationId,
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId
                );
                
                DocumentSession.Events.Append(cmd.DeviceStructuredInformationId, @event);
                DocumentSession.SaveChanges();
                
                return CommandResult.Success;
            }
        }
    }
}