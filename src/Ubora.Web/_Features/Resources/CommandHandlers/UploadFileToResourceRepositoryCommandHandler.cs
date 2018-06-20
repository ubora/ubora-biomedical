using Marten;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Resources.Events;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.ResourceRepository.Commands
{
    public class UploadFileToResourceRepositoryCommandHandler : ICommandHandler<UploadFileToResourceRepositoryCommand>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IUboraStorageProvider _uboraStorageProvider;

        public UploadFileToResourceRepositoryCommandHandler(IDocumentSession documentSession, IUboraStorageProvider uboraStorageProvider, IDocumentStore documentStore)
        {
            _documentSession = documentSession;
            _uboraStorageProvider = uboraStorageProvider;
        }

        public ICommandResult Handle(UploadFileToResourceRepositoryCommand cmd)
        {
            var resourcePage = _documentSession.LoadOrThrow<ResourcePage>(cmd.ResourcePageId);

            var blobLocation = 
                new BlobLocation(
                    containerName: BlobLocation.ContainerNames.Resources,
                    blobPath: $"pages/{resourcePage.Id}/repository/{cmd.FileId}/{cmd.FileName}");

            _uboraStorageProvider
                .SavePublic(blobLocation, cmd.FileStream)
                .GetAwaiter().GetResult();

            var @event = 
                new ResourceFileUploadedEvent(
                    initiatedBy: cmd.Actor,
                    fileId: cmd.FileId,
                    resourcePageId: resourcePage.Id,
                    blobLocation: blobLocation,
                    fileName: cmd.FileName,
                    fileSize: cmd.FileSize);

            _documentSession.Events.Append(resourcePage.Id, @event);
            _documentSession.SaveChanges();

            return CommandResult.Success;
        }
    }
}