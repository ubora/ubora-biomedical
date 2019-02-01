using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.StructuredInformations.Events;

namespace Ubora.Domain.Projects.StructuredInformations.Commands
{
    public class EditHealthTechnologySpecificationInformationCommand : UserProjectCommand
    {
        public Guid DeviceStructuredInformationId { get; set; }
        public DeviceStructuredInformationWorkpackageTypes WorkpackageType { get; set; }
        public HealthTechnologySpecificationsInformation HealthTechnologySpecificationsInformation { get; set; }

        internal class Handler : CommandHandler<EditHealthTechnologySpecificationInformationCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditHealthTechnologySpecificationInformationCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var deviceStructuredInformation = DocumentSession.LoadOrThrow<DeviceStructuredInformation>(cmd.DeviceStructuredInformationId);

                var @event = new HealthTechnologySpecificationInformationWasEditedEvent(initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    deviceStructuredInformationId: cmd.DeviceStructuredInformationId, workpackageType: cmd.WorkpackageType, healthTechnologySpecificationsInformation: cmd.HealthTechnologySpecificationsInformation);

                DocumentSession.Events.Append(cmd.DeviceStructuredInformationId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
