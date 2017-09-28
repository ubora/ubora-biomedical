using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class ProjectCreatedEvent : UboraEvent
    {
        public ProjectCreatedEvent(UserInfo initiatedBy, Guid id, string title, string clinicalNeed, string areaOfUsage, string potentialTechnology, string gmdn) : base(initiatedBy)
        {
            Id = id;
            Title = title;
            ClinicalNeed = clinicalNeed;
            AreaOfUsage = areaOfUsage;
            PotentialTechnology = potentialTechnology;
            Gmdn = gmdn;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ClinicalNeed { get; set; }
        public string AreaOfUsage { get; set; }
        public string PotentialTechnology { get; set; }
        public string Gmdn { get; set; }

        public override string GetDescription()
        {
            return $"created project \"{StringTokens.Project(Id)}\".";
        }
    }
}
