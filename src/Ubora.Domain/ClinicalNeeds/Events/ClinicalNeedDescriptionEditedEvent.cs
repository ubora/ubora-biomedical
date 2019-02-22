using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.ClinicalNeeds.Events
{
    public class ClinicalNeedDescriptionEditedEvent : UboraEvent
    {
        public ClinicalNeedDescriptionEditedEvent(UserInfo initiatedBy, QuillDelta description) : base(initiatedBy)
        {
            Description = description;
        }

        public QuillDelta Description { get; }

        public override string GetDescription() => "edited description.";
    }
}
