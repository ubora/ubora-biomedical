using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.IsoStandardsCompliances.Events;

namespace Ubora.Domain.Projects.IsoStandardsCompliances.Commands
{
    public class AddIsoStandardCommand : UserProjectCommand
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public Uri Link { get; set; }

        public class Handler : ICommandHandler<AddIsoStandardCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AddIsoStandardCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var isoStandardId = Guid.NewGuid();

                var @event = new IsoStandardAddedToComplianceChecklistEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, isoStandardId, cmd.Title, cmd.ShortDescription, cmd.Link);
                _documentSession.Events.Append(cmd.ProjectId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}