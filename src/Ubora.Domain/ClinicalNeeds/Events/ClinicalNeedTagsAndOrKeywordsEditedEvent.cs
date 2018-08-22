using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.ClinicalNeeds.Events
{
    public class ClinicalNeedTagsAndOrKeywordsEditedEvent : UboraEvent
    {
        public ClinicalNeedTagsAndOrKeywordsEditedEvent(UserInfo initiatedBy, string clinicalNeedTag, string areaOfUsageTag, string potentialTechnologyTag, string keywords) : base(initiatedBy)
        {
            ClinicalNeedTag = clinicalNeedTag;
            AreaOfUsageTag = areaOfUsageTag;
            PotentialTechnologyTag = potentialTechnologyTag;
            Keywords = keywords;
        }

        public string ClinicalNeedTag { get; }
        public string AreaOfUsageTag { get; }
        public string PotentialTechnologyTag { get; }
        public string Keywords { get; }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}