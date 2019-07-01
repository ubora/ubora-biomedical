using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.CommercialDossiers.Events
{
    public abstract class CommercialDossierEventBase : ProjectEvent 
    {
        protected CommercialDossierEventBase(UserInfo initiatedBy, Guid projectId, Guid aggregateId)
            : base(initiatedBy, projectId)
        {
            AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }
    }
}