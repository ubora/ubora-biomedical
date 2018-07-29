using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Events
{
    public class IsoStandardRemovedFromComplianceChecklistEvent : ProjectEvent
    {
        public IsoStandardRemovedFromComplianceChecklistEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, Guid isoStandardId) : base(initiatedBy, projectId)
        {
            AggregateId = aggregateId;
            IsoStandardId = isoStandardId;
        }

        public Guid AggregateId { get; }
        public Guid IsoStandardId { get; set; }

        public override string GetDescription() => "removed ISO standard from the compliance checklist.";
    }
}
