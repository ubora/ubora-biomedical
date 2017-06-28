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
            var project = _documentSession.Load<Project>(cmd.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var blobProperties = new BlobProperties
            {
                Security = BlobSecurity.Public
            };

            var fileName = $"{project.Id}/repository/{cmd.FileName}";
            _storageProvider.SaveBlobStreamAsync("projects", fileName, cmd.Stream, blobProperties)
                .Wait();

            var fileLocation = _storageProvider.GetBlobUrl("projects", fileName);

            var @event = new FileAddedEvent(
                cmd.Actor,
                cmd.ProjectId,
                cmd.Id,
                fileName,
                fileLocation
            );

            _documentSession.Events.Append(cmd.ProjectId, @event);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
