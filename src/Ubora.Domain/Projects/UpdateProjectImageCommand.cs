using Marten;
using System;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class UpdateProjectImageCommand : UserProjectCommand
    {
    }

    internal class UpdateProjectImageCommandHandler : ICommandHandler<UpdateProjectImageCommand>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IStorageProvider _storageProvider;

        public UpdateProjectImageCommandHandler(IDocumentSession documentSession, IStorageProvider storageProvider)
        {
            _documentSession = documentSession;
            _storageProvider = storageProvider;
        }

        public ICommandResult Handle(UpdateProjectImageCommand cmd)
        {
            var project = _documentSession.Load<Project>(cmd.ProjectId);

            var @event = new ProjectImageUpdatedEvent(DateTime.UtcNow, cmd.Actor);

            _documentSession.Events.Append(project.Id, @event);
            _documentSession.SaveChanges();


            return new CommandResult();
        }
    }
}
