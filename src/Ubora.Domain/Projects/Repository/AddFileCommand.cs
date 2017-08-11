using Marten;
using System;
using System.IO;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Repository
{
    public class AddFileCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public Stream Stream { get; set; }
        public string FileName { get; set; }

        internal class Handler : ICommandHandler<AddFileCommand>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IStorageProvider _storageProvider;

            public Handler(IDocumentSession documentSession, IStorageProvider storageProvider)
            {
                _documentSession = documentSession;
                _storageProvider = storageProvider;
            }

            public ICommandResult Handle(AddFileCommand cmd)
            {
                var project = _documentSession.Load<Project>(cmd.ProjectId);
                if (project == null)
                {
                    throw new InvalidOperationException();
                }

                var blobProperties = new BlobProperties
                {
                    Security = BlobSecurity.Public
                };

                var blobLocation = new BlobLocation(
                    containerName: BlobLocation.ContainerNames.Projects,
                    blobPath: $"{project.Id}/repository/{Guid.NewGuid()}/{cmd.FileName}");

                // TODO: Refactor this
                _storageProvider.SaveBlobStreamAsync(blobLocation.ContainerName, blobLocation.BlobPath, cmd.Stream, blobProperties)
                    .GetAwaiter()
                    .GetResult();

                var @event = new FileAddedEvent(
                    cmd.Actor,
                    cmd.ProjectId,
                    cmd.Id,
                    cmd.FileName,
                    blobLocation
                );

                _documentSession.Events.Append(cmd.ProjectId, @event);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
