using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.CommercialDossiers.Events
{
    public class DescriptionEditedEvent : CommercialDossierEventBase
    {
        public DescriptionEditedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, QuillDelta value) 
            : base(initiatedBy, projectId, aggregateId)
        {
            Value = value;
        }

        public QuillDelta Value { get; }

        public override string GetDescription() => "edited commercial description.";
    }
}