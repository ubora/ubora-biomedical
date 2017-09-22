using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Repository.Events;

namespace Ubora.Domain.Projects.Repository.Commands
{
    public class HideFileCommand : UserProjectCommand
    {
        public Guid Id { get; set; }

        internal class Handler : ICommandHandler<HideFileCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(HideFileCommand cmd)
            {
                var file = _documentSession.LoadOrThrow<ProjectFile>(cmd.Id);

                var @event = new FileHiddenEvent(
                    cmd.Actor,
                    cmd.Id
                );

                _documentSession.Events.Append(file.ProjectId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
