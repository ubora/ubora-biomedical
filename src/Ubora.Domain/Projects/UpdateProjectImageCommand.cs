using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class UpdateProjectImageCommand : UserProjectCommand
    {
        internal class Handler : ICommandHandler<UpdateProjectImageCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
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
}
