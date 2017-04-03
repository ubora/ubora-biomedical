using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using Ubora.Domain.Events;

namespace Ubora.Domain.Projects.Events
{
    public class ProjectCreated : UboraEvent 
    {
        public string Name { get; }
        public ProjectCreated(string name, UserInfo creator) : base(creator)
        {
            Name = name;
        }

        public override string Description()
        {
            return $"Project created \"{Name}\"";
        }
    }

  
}
