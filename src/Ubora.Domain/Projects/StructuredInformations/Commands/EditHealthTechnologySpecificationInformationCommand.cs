using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.StructuredInformations.Events;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Projects.StructuredInformations.Commands
{
    public class EditHealthTechnologySpecificationInformationCommand : UserProjectCommand
    {
        public HealthTechnologySpecificationsInformation HealthTechnologySpecificationsInformation { get; set; }

        internal class Handler : CommandHandler<EditHealthTechnologySpecificationInformationCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditHealthTechnologySpecificationInformationCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var wp2 = DocumentSession.LoadOrThrow<WorkpackageTwo>(cmd.ProjectId);

                var @event = new HealthTechnologySpecificationInformationWasEditedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    healthTechnologySpecificationsInformation: cmd.HealthTechnologySpecificationsInformation);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
