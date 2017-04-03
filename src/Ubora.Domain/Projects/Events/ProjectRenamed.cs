using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Events;

namespace Ubora.Domain.Projects.Events
{
    public class ProjectRenamed : UboraEvent
    {
        public string NewName { get; private set; }
        public ProjectRenamed(string name, UserInfo creator) : base(creator)
        {
            NewName = name;
        }

        public override string Description()
        {
            return $"Project renamed to \"{NewName}\"";
        }
    }
}
