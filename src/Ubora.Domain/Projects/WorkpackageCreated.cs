using System;
using Ubora.Domain.Events;

namespace Ubora.Domain.Projects
{
    public class WorkpackageCreated : UboraEvent
    {
        public Guid Id { get; set; }

        public string Name { get; }

        public WorkpackageCreated(string name, UserInfo creator) : base(creator)
        {
            Name = name;
        }

        public override string Description()
        {
            return $"Workpackage added \"{Name}\"";
        }
    }
}
