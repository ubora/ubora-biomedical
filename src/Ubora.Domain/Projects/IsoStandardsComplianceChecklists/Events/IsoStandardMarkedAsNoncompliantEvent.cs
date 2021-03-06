﻿using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Events
{
    public class IsoStandardMarkedAsNoncompliantEvent : ProjectEvent
    {
        public IsoStandardMarkedAsNoncompliantEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, Guid isoStandardId) : base(initiatedBy, projectId)
        {
            AggregateId = aggregateId;
            IsoStandardId = isoStandardId;
        }

        public Guid AggregateId { get; }
        public Guid IsoStandardId { get; }

        public override string GetDescription() => "marked ISO standard as non-compliant.";
    }
}
