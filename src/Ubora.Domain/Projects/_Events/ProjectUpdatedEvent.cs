using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class ProjectUpdatedEvent : ProjectEvent
    {
        public ProjectUpdatedEvent(UserInfo initiatedBy, Guid projectId, string title, string clinicalNeedTags, string areaOfUsageTags, string potentialTechnologyTags, string gmdn) : base(initiatedBy, projectId)
        {
            Title = title;
            ClinicalNeedTags = clinicalNeedTags;
            AreaOfUsageTags = areaOfUsageTags;
            PotentialTechnologyTags = potentialTechnologyTags;
            Gmdn = gmdn;
        }

        public string Title { get; private set; }
        public string ClinicalNeedTags { get; private set; }
        public string AreaOfUsageTags { get; private set; }
        public string PotentialTechnologyTags { get; private set; }
        public string Gmdn { get; private set; }

        public override string GetDescription()
        {
            return $"updated project \"{StringTokens.Project(ProjectId)}\" details.";
        }
    }
}