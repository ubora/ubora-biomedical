using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.IsoStandardsCompliances.Events
{
    public class IsoStandardRemovedFromChecklistEvent : ProjectEvent
    {
        public IsoStandardRemovedFromChecklistEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, Guid isoStandardId) : base(initiatedBy, projectId)
        {
            AggregateId = aggregateId;
            IsoStandardId = isoStandardId;
        }

        public Guid AggregateId { get; }
        public Guid IsoStandardId { get; set; }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}
