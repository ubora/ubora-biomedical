using Ubora.Domain.Events;

namespace Ubora.Domain.Projects
{
    public class ProjectCreatedEvent : UboraEvent 
    {
        public string Name { get; }

        public ProjectCreatedEvent(string name, UserInfo creator) : base(creator)
        {
            Name = name;
        }

        public override string Description()
        {
            return $"Project created \"{Name}\"";
        }
    }
}
