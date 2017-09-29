using System;
using Marten;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Repository.Events;

namespace Ubora.Domain.Projects.Repository.Commands
{
    public class AddFileCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public BlobLocation BlobLocation { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
        public long FileSize { get; set; }
        public string FolderName { get; set; }

        internal class Handler : ICommandHandler<AddFileCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AddFileCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var @event = new FileAddedEvent(
                    id: cmd.Id,
                    projectId: cmd.ProjectId,
                    location: cmd.BlobLocation,
                    comment: cmd.Comment,
                    fileSize: cmd.FileSize,
                    initiatedBy: cmd.Actor,
                    fileName: cmd.FileName,
                    folderName: cmd.FolderName,
                    revisionNumber: 1
                );

                _documentSession.Events.Append(cmd.ProjectId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
