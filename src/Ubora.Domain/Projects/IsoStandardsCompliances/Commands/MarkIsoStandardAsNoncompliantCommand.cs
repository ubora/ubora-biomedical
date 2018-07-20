using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.IsoStandardsCompliances.Events;

namespace Ubora.Domain.Projects.IsoStandardsCompliances.Commands
{
    public class MarkIsoStandardAsNoncompliantCommand : UserProjectCommand
    {
        public Guid IsoStandardId { get; set; }

        public class Handler : ICommandHandler<MarkIsoStandardAsNoncompliantCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(MarkIsoStandardAsNoncompliantCommand cmd)
            {
                var aggregate = _documentSession.LoadOrThrow<IsoStandardsComplianceAggregate>(cmd.ProjectId);

                _documentSession.Events.Append(aggregate.Id, new IsoStandardMarkedAsNoncompliantEvent(cmd.Actor, cmd.ProjectId, cmd.ProjectId, cmd.IsoStandardId));
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}