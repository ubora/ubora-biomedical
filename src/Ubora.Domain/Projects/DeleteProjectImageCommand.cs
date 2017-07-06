using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
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

                var @event = new ProjectImageDeletedEvent(cmd.Actor);

                _documentSession.Events.Append(project.Id, @event);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
