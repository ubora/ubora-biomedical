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
            private readonly ICommandProcessor _commandProcessor;
            
            public Handler(IDocumentSession documentSession, ICommandProcessor commandProcessor) : base(documentSession)
            {
                _commandProcessor = commandProcessor;
            }

            public override ICommandResult Handle(OpenWorkpackageFourCommand cmd)
            {
                var workpackageOne = DocumentSession.LoadOrThrow<WorkpackageOne>(cmd.ProjectId);
                if (!workpackageOne.HasBeenAccepted)
                {
                    return CommandResult.Failed("Work package one hasn't been accepted.");
                } 
                var workpackageTwo = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);
 
                var workPackageFour = DocumentSession.Load<WorkpackageFour>(cmd.ProjectId);
                if (workPackageFour != null)
                {
                    return CommandResult.Failed("Work package is already opened.");
                }
                
                var result = _commandProcessor.Execute(new OpenWorkpackageThreeCommand { Actor = cmd.Actor, ProjectId = cmd.ProjectId});
                if(result.IsFailure)
                {
                    return CommandResult.Failed("Work package three has been failure.");
                }

                var workPackageThree = DocumentSession.Load<WorkpackageThree>(cmd.ProjectId);
                if (!workPackageThree.HasBeenOpened)
                {
                    return CommandResult.Failed("Work package three hasn't been opened.");
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