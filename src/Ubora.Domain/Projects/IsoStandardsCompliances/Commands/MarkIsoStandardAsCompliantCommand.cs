using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.IsoStandardsCompliances.Events;

namespace Ubora.Domain.Projects.IsoStandardsCompliances.Commands
{
    public class MarkIsoStandardAsCompliantCommand : UserProjectCommand
    {
        public Guid IsoStandardId { get; set; }

        public class Handler : ICommandHandler<MarkIsoStandardAsCompliantCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(MarkIsoStandardAsCompliantCommand cmd)
            {
                var aggregate = _documentSession.LoadOrThrow<IsoStandardsComplianceAggregate>(cmd.ProjectId);

                _documentSession.Events.Append(aggregate.Id, new IsoStandardMarkedAsCompliantEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.IsoStandardId));
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}