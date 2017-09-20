using System;
using Marten;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public class UpdateProjectImageCommand : UserProjectCommand
    {
        public BlobLocation BlobLocation { get; set; }

        internal class Handler : ICommandHandler<UpdateProjectImageCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(UpdateProjectImageCommand cmd)
            {
                var project = _documentSession.Load<Project>(cmd.ProjectId);

                var @event = new ProjectImageUpdatedEvent(cmd.BlobLocation, DateTime.UtcNow, cmd.Actor);

                _documentSession.Events.Append(project.Id, @event);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
