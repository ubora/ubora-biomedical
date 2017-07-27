using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Repository
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
                var file = _documentSession.Load<ProjectFile>(cmd.Id);
                if (file == null)
                {
                    throw new InvalidOperationException();
                }

                var @event = new FileHidEvent(
                    cmd.Actor,
                    cmd.Id,
                    file.FileName
                );

                _documentSession.Events.Append(file.ProjectId, @event);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
