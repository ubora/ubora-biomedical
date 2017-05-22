using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.DeviceClassification
{
    internal class SaveDeviceClassificationToProjectCommandHandler : CommandHandler<SaveDeviceClassificationToProjectCommand>
    {
        public SaveDeviceClassificationToProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(SaveDeviceClassificationToProjectCommand cmd)
        {
            var project = DocumentSession.Load<Project>(cmd.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var @event = new DeviceClassificationSavedEvent(cmd.UserInfo)
            {
                Id = cmd.Id,
                DeviceClassification = cmd.DeviceClassification,
                ProjectId = cmd.ProjectId
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
