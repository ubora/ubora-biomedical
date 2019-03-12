using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.CommercialDossiers.Events
{
    public class CommercialNameChangedEvent : CommercialDossierEventBase
    {
        public CommercialNameChangedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, string value) 
            : base(initiatedBy, projectId, aggregateId)
        {
            Value = value;
        }

        public string Value  { get; }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}