using Marten;
using System.IO;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class UpdateProjectImageCommand : UserProjectCommand
    {
        public Stream Stream { get; set; }
        public string ImageName { get; set; }
    }

    internal class UpdateProjectImageCommandHandler : ICommandHandler<UpdateProjectImageCommand>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IStorageProvider _storageProvider;

        public UpdateProjectImageCommandHandler(IDocumentSession documentSession, IStorageProvider storageProvider)
        {
            _documentSession = documentSession;
            _storageProvider = storageProvider;
        }

        public ICommandResult Handle(UpdateProjectImageCommand cmd)
        {
            var blobProperties = new BlobProperties
            {
                Security = BlobSecurity.Public
            };

            var project = _documentSession.Load<Project>(cmd.ProjectId);

            if (!string.IsNullOrEmpty(project.ImageBlobName))
            {
                _storageProvider.DeleteBlobAsync("projects", $"{cmd.ProjectId}/project-image/{project.ImageBlobName}");
            }

            _storageProvider.SaveBlobStreamAsync("projects", $"{cmd.ProjectId}/project-image/{cmd.ImageName}", cmd.Stream, blobProperties)
                .Wait();

            var @event = new ProjectImageUpdatedEvent(cmd.Actor) { ImageName = cmd.ImageName };

            _documentSession.Events.Append(project.Id, @event);
            _documentSession.SaveChanges();


            return new CommandResult();
        }
    }
}
