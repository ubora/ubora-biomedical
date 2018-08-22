using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.ClinicalNeeds.Events
{
    public class ClinicalNeedIndicatedEvent : UboraEvent
    {
        public ClinicalNeedIndicatedEvent(UserInfo initiatedBy, Guid clinicalNeedId, string title, QuillDelta description, string clinicalNeedTag, string areaOfUsageTag, string potentialTechnologyTag, string keywords)
            : base(initiatedBy)
        {
            ClinicalNeedId = clinicalNeedId;
            Title = title;
            Description = description;
            ClinicalNeedTag = clinicalNeedTag;
            AreaOfUsageTag = areaOfUsageTag;
            PotentialTechnologyTag = potentialTechnologyTag;
            Keywords = keywords;
        }

        public Guid ClinicalNeedId { get; }
        public string Title { get; }
        public QuillDelta Description { get; }
        public string ClinicalNeedTag { get; }
        public string AreaOfUsageTag { get; }
        public string PotentialTechnologyTag { get; }
        public string Keywords { get; }

        public override string GetDescription() => "indicated clinical need.";
    }
}