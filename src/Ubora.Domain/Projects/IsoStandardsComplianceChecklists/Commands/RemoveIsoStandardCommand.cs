﻿using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Events;

namespace Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Commands
{
    public class RemoveIsoStandardCommand : UserProjectCommand
    {
        public Guid IsoStandardId { get; set; }

        public class Handler : ICommandHandler<RemoveIsoStandardCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(RemoveIsoStandardCommand cmd)
            {
                var aggregate = _documentSession.LoadOrThrow<IsoStandardsComplianceChecklist>(cmd.ProjectId);

                var @event = new IsoStandardRemovedFromComplianceChecklistEvent(cmd.Actor, cmd.ProjectId, aggregate.Id, cmd.IsoStandardId);
                _documentSession.Events.Append(aggregate.Id, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}