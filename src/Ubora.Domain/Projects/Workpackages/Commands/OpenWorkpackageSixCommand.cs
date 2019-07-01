using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages.Commands
{
    public class OpenWorkpackageSixCommand : UserProjectCommand
    {
        public class Handler : ICommandHandler<OpenWorkpackageSixCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(OpenWorkpackageSixCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var id = Guid.NewGuid();
                _documentSession.Events.Append(project.Id, new WorkpackageSixOpenedEvent(cmd.Actor, cmd.ProjectId));
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}