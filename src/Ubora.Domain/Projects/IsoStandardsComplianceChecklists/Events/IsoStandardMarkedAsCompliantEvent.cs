using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Events
{
    public class IsoStandardMarkedAsCompliantEvent : ProjectEvent
    {
        public IsoStandardMarkedAsCompliantEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, Guid isoStandardId) : base(initiatedBy, projectId)
        {
            AggregateId = aggregateId;
            IsoStandardId = isoStandardId;
        }

        public Guid AggregateId { get; set; }
        public Guid IsoStandardId { get; set; }

        public override string GetDescription() => "marked ISO standard as compliant.";
    }
}
