using System;
using Marten;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Repository.Events;

namespace Ubora.Domain.Projects.Repository.Commands
{
    public class UpdateFileCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public BlobLocation BlobLocation { get; set; }
        public string Comment { get; set; }
        public long FileSize { get; set; }

        internal class Handler : ICommandHandler<UpdateFileCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(UpdateFileCommand cmd)
            {
                var projectFile = _documentSession.LoadOrThrow<ProjectFile>(cmd.Id);

                var revisionNumber = projectFile.RevisionNumber + 1;
                var @event = new FileUpdatedEvent(
                    projectFile.Id,
                    projectFile.ProjectId,
                    cmd.BlobLocation,
                    cmd.Comment,
                    cmd.FileSize,
                    cmd.Actor,
                    revisionNumber
                );

                _documentSession.Events.Append(projectFile.ProjectId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
