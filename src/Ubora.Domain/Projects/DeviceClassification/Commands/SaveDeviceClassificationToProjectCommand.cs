using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.DeviceClassification.Events;

namespace Ubora.Domain.Projects.DeviceClassification.Commands
{
    public class SetDeviceClassificationForProjectCommand : UserProjectCommand
    {
        public Classification DeviceClassification { get; set; }

        internal class Handler : CommandHandler<SetDeviceClassificationForProjectCommand>
        {
            private readonly IDeviceClassification _deviceClassification;

            public Handler(
                IDocumentSession documentSession,
                IDeviceClassificationProvider deviceClassificationProvider) : base(documentSession)
            {
                _deviceClassification = deviceClassificationProvider.Provide();
            }

            public override ICommandResult Handle(SetDeviceClassificationForProjectCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var currentClassification = string.IsNullOrEmpty(project.DeviceClassification) ? null : _deviceClassification.GetClassification(project.DeviceClassification);

                var @event = new EditedProjectDeviceClassificationEvent(
                    projectId: cmd.ProjectId, 
                    newClassification: cmd.DeviceClassification, 
                    currentClassification: currentClassification, 
                    initiatedBy: cmd.Actor);

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }

}
