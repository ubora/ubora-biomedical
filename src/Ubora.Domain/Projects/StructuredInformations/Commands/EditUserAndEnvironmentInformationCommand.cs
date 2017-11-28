using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.StructuredInformations.Events;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Projects.StructuredInformations.Commands
{
    public class EditUserAndEnvironmentInformationCommand : UserProjectCommand
    {
        public UserAndEnvironmentInformation UserAndEnvironmentInformation { get; set; }

        internal class Handler : CommandHandler<EditUserAndEnvironmentInformationCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditUserAndEnvironmentInformationCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var wp2 = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var @event = new UserAndEnvironmentInformationWasEditedEvent(
                    initiatedBy: cmd.Actor, 
                    projectId: cmd.ProjectId, 
                    userAndEnvironmentInformation: cmd.UserAndEnvironmentInformation);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
