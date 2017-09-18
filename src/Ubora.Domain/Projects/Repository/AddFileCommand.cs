using Marten;
using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Repository
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
                var project = _documentSession.Load<Project>(cmd.ProjectId);
                if (project == null)
                {
                    throw new InvalidOperationException();
                }

                var @event = new FileAddedEvent(
                    cmd.Id,
                    cmd.ProjectId,
                    cmd.BlobLocation,
                    cmd.Comment,
                    cmd.FileSize,
                    cmd.Actor,
                    cmd.FileName,
                    cmd.FolderName
                );

                _documentSession.Events.Append(cmd.ProjectId, @event);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
