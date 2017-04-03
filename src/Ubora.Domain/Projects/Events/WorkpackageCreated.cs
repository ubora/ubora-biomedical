using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Events;

namespace Ubora.Domain.Projects.Events
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
