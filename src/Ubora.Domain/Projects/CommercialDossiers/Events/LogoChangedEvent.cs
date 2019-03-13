using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.CommercialDossiers.Events
{
    public class LogoChangedEvent : CommercialDossierEventBase
    {
        public LogoChangedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, BlobLocation value)
            : base(initiatedBy, projectId, aggregateId)
        {
            Value = value;
        }

        public BlobLocation Value { get; }

        public override string GetDescription() => "changed commercial logo.";
    }
}