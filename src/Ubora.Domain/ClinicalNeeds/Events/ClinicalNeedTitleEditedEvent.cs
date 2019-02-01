using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.ClinicalNeeds.Events
{
    public class ClinicalNeedTitleEditedEvent : UboraEvent
    {
        public ClinicalNeedTitleEditedEvent(UserInfo initiatedBy, string title) : base(initiatedBy)
        {
            Title = title;
        }

        public string Title { get; }

        public override string GetDescription() => "edited title.";
    }
}