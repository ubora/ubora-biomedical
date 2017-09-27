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
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var @event = new ProjectImageDeletedEvent(
                    initiatedBy: cmd.Actor, 
                    when: DateTime.UtcNow);

                _documentSession.Events.Append(project.Id, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
