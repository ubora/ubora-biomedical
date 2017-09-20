using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    public class ProjectCreatedEvent : UboraEvent 
    {
        public ProjectCreatedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
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
