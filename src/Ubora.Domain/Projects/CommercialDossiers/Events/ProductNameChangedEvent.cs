using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.CommercialDossiers.Events
{
    public class ProductNameChangedEvent : CommercialDossierEventBase
    {
        public ProductNameChangedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, string value)
            : base(initiatedBy, projectId, aggregateId)
        {
            Value = value;
        }

        public string Value { get; }

        public override string GetDescription() => $"changed product name to \"{Value}\"";
    }
}