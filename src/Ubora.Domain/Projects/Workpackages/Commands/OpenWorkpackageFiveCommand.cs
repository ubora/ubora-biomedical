using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class OpenWorkpackageFiveCommand : UserProjectCommand
    {
        public class Handler : ICommandHandler<OpenWorkpackageFiveCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(OpenWorkpackageFiveCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var id = Guid.NewGuid();
                _documentSession.Events.Append(project.Id, new WorkpackageFiveOpenedEvent(cmd.Actor, cmd.ProjectId));
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
