using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.DeviceClassification
{
    internal class SaveDeviceClassificationToProjectCommandHandler : CommandHandler<SetDeviceClassificationForProjectCommand>
    {
        private IDeviceClassification _deviceClassification;

        public SaveDeviceClassificationToProjectCommandHandler(IDocumentSession documentSession, IDeviceClassification deviceClassification) : base(documentSession)
        {
            _deviceClassification = deviceClassification;
        }

        public override ICommandResult Handle(SetDeviceClassificationForProjectCommand cmd)
        {
            var project = DocumentSession.Load<Project>(cmd.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var currentClassification = string.IsNullOrEmpty(project.DeviceClassification) ? null : _deviceClassification.GetClassification(project.DeviceClassification);

            var @event = new DeviceClassificationSetEvent(cmd.ProjectId, cmd.DeviceClassification, currentClassification, cmd.Actor);

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
