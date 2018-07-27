using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.StructuredInformations.Events;

namespace Ubora.Domain.Projects.StructuredInformations.Commands
{
    public class EditUserAndEnvironmentInformationCommand : UserProjectCommand
    {
        public Guid DeviceStructuredInformationId { get; set; }
        public DeviceStructuredInformationWorkpackageTypes WorkpackageType { get; set; }
        public UserAndEnvironmentInformation UserAndEnvironmentInformation { get; set; }

        internal class Handler : CommandHandler<EditUserAndEnvironmentInformationCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditUserAndEnvironmentInformationCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var deviceStructuredInformation = DocumentSession.LoadOrThrow<DeviceStructuredInformation>(cmd.DeviceStructuredInformationId);

                var @event = new UserAndEnvironmentInformationWasEditedEvent(initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    deviceStructuredInformationId: cmd.DeviceStructuredInformationId, workpackageType: cmd.WorkpackageType, userAndEnvironmentInformation: cmd.UserAndEnvironmentInformation);

                DocumentSession.Events.Append(cmd.DeviceStructuredInformationId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
