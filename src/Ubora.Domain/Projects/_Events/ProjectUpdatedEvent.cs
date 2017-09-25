using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class ProjectUpdatedEvent : UboraEvent
    {
        public ProjectUpdatedEvent(UserInfo initiatedBy, Guid id, string title, string clinicalNeedTags, string areaOfUsageTags, string potentialTechnologyTags, string gmdn) : base(initiatedBy)
        {
            Id = id;
            Title = title;
            ClinicalNeedTags = clinicalNeedTags;
            AreaOfUsageTags = areaOfUsageTags;
            PotentialTechnologyTags = potentialTechnologyTags;
            Gmdn = gmdn;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ClinicalNeedTags { get; set; }
        public string AreaOfUsageTags { get; set; }
        public string PotentialTechnologyTags { get; set; }
        public string Gmdn { get; set; }

        public override string GetDescription()
        {
            return $"updated project \"{StringTokens.Project(Id)}\" details.";
        }
    }
}