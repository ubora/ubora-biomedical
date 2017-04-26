using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    public class WorkpackageCreatedEvent : UboraEvent
    {
        public Guid Id { get; set; }

        public string Name { get; }

        public WorkpackageCreatedEvent(string name, UserInfo creator) : base(creator)
        {
            Name = name;
        }

        public override string GetDescription()
        {
            return $"Workpackage added \"{Name}\"";
        }
    }
}
