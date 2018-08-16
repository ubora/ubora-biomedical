using System;
using Ubora.Domain.ClinicalNeeds.Events;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.ClinicalNeeds
{
    public class ClinicalNeed : Entity<ClinicalNeed>, ITagsAndKeywords
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ClinicalNeedTag { get; private set; }
        public string AreaOfUsageTag { get; private set; }
        public string PotentialTechnologyTag { get; private set; }
        public string Keywords { get; private set; }
        public DateTimeOffset IndicatedAt { get; private set; }
        public Guid IndicatorUserId { get; set; }

        private void Apply(ClinicalNeedIndicatedEvent @event)
        {
            if (IndicatedAt != default(DateTimeOffset))
                throw new InvalidOperationException();

            Id = @event.ClinicalNeedId;
            Title = @event.Title;
            Description = @event.Description;
            ClinicalNeedTag = @event.ClinicalNeedTag;
            AreaOfUsageTag = @event.AreaOfUsageTag;
            PotentialTechnologyTag = @event.PotentialTechnologyTag;
            Keywords = @event.Keywords;
            IndicatedAt = @event.Timestamp;
            IndicatorUserId = @event.InitiatedBy.UserId;
        }
    }
}
