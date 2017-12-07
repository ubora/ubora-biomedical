using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Repository.Events;
using Ubora.Domain.Questionnaires.DeviceClassifications;

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
                    id: projectFile.Id,
                    projectId: projectFile.ProjectId,
                    location: cmd.BlobLocation,
                    comment: cmd.Comment,
                    fileSize: cmd.FileSize,
                    initiatedBy: cmd.Actor,
                    revisionNumber: revisionNumber
                );

                _documentSession.Events.Append(projectFile.ProjectId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
