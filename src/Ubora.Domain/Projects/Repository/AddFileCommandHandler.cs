using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using TwentyTwenty.Storage;

namespace Ubora.Domain.Projects.Repository
{
    public class AddFileCommandHandler : ICommandHandler<AddFileCommand>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IStorageProvider _storageProvider;

        public AddFileCommandHandler(IDocumentSession documentSession, IStorageProvider storageProvider)
        {
            _documentSession = documentSession;
            _storageProvider = storageProvider;
        }

        public ICommandResult Handle(AddFileCommand cmd)
        {
            var blobProperties = new BlobProperties
            {
                Security = BlobSecurity.Public
            };

            var project = _documentSession.Load<Project>(cmd.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var @event = new FileAddedEvent(cmd.Actor)
            {
                ProjectId = cmd.ProjectId,
                Id = cmd.Id,
                FileName = cmd.FileName,
            };

            _documentSession.Events.Append(cmd.ProjectId, @event);
            _documentSession.SaveChanges();

            _storageProvider.SaveBlobStreamAsync($"projects/{project.Id}/files", cmd.FileName, cmd.Stream, blobProperties)
                .Wait();

            return new CommandResult();
        }
    }
}
