using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class DeleteProjectCommand : UserProjectCommand
    {
        internal class Handler : ICommandHandler<DeleteProjectCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeleteProjectCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var @event = new ProjectDeletedEvent(cmd.Actor, project.Id);
                _documentSession.Events.Append(project.Id, @event);
                _documentSession.Delete<Project>(project.Id);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
