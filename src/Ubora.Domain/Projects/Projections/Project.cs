﻿using Marten.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Projects.Events;

namespace Ubora.Domain.Projects.Projections
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public void Apply(Event<ProjectCreated> created)
        {
            Name = created.Data.Name;
        }

        public void Apply(Event<ProjectRenamed> renamed)
        {
            Name = renamed.Data.NewName;
        }

        public override string ToString()
        {
            return $"Project(Id:{Id})";
        }
    }
}
