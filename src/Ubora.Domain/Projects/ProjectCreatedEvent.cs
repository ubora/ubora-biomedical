using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    public class ProjectCreatedEvent : UboraEvent 
    {
        public string Title { get; }
        public string Description { get; }
        public string ClinicalNeed { get; }
        public string AreaOfUsage { get; }
        public string PotentialTechnology { get; }
        public string GmdnTerm { get; }
        public string GmdnDefinition { get; }
        public string GmdnCode { get; }

        public ProjectCreatedEvent(UserInfo creator, string title, string description, string clinicalNeed, string areaOfUsage, string potentialTechnology, string gmdnTerm, string gmdnDefinition, string gmdnCode) : base(creator)
        {
            Title = title;
            Description = description;
            ClinicalNeed = clinicalNeed;
            AreaOfUsage = areaOfUsage;
            PotentialTechnology = potentialTechnology;
            GmdnTerm = gmdnTerm;
            GmdnDefinition = gmdnDefinition;
            GmdnCode = gmdnCode;
        }

        public override string GetDescription()
        {
            return $"Project created \"{Title}\"";
        }
    }
}
