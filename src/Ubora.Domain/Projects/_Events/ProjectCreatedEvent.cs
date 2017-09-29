using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class ProjectCreatedEvent : ProjectEvent 
    {
        public ProjectCreatedEvent(UserInfo initiatedBy, Guid projectId, string title, string clinicalNeed, string areaOfUsage, string potentialTechnology, string gmdn) : base(initiatedBy, projectId)
        {
            Title = title;
            ClinicalNeed = clinicalNeed;
            AreaOfUsage = areaOfUsage;
            PotentialTechnology = potentialTechnology;
            Gmdn = gmdn;
        }

        public string Title { get; private set; }
        public string ClinicalNeed { get; private set; }
        public string AreaOfUsage { get; private set; }
        public string PotentialTechnology { get; private set; }
        public string Gmdn { get; private set; }

        public override string GetDescription()
        {
            return $"created project \"{StringTokens.Project(ProjectId)}\".";
        }
    }
}
