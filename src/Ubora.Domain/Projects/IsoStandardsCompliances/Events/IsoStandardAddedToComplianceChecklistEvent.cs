using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.IsoStandardsCompliances.Events
{
    public class IsoStandardAddedToComplianceChecklistEvent : ProjectEvent
    {
        public IsoStandardAddedToComplianceChecklistEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, string title, string shortDescription, Uri link)
            : base(initiatedBy, projectId)
        {
            AggregateId = aggregateId;
            Title = title;
            ShortDescription = shortDescription;
            Link = link;
        }

        public Guid AggregateId { get; }
        public string Title { get; }
        public string ShortDescription { get; }
        public Uri Link { get; }

        public override string GetDescription() => "added ISO standard to compliance checklist.";
    }
}
