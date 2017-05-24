using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.DeviceClassification
{
    internal class SaveDeviceClassificationToProjectCommandHandler : CommandHandler<SetDeviceClassificationForProjectCommand>
    {
        public SaveDeviceClassificationToProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(SetDeviceClassificationForProjectCommand cmd)
        {
            var project = DocumentSession.Load<Project>(cmd.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var @event = new DeviceClassificationSetEvent(cmd.ProjectId, cmd.DeviceClassification, cmd.Actor);

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
