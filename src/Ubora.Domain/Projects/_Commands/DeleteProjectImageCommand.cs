using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class DeleteProjectImageCommand : UserProjectCommand
    {
        internal class Handler : ICommandHandler<DeleteProjectImageCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeleteProjectImageCommand cmd)
            {
                var project = _documentSession.Load<Project>(cmd.ProjectId);

                var @event = new ProjectImageDeletedEvent(DateTime.UtcNow, cmd.Actor);

                _documentSession.Events.Append(project.Id, @event);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
