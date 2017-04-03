using Marten.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Projects.Events;

namespace Ubora.Domain.Projects.Projections
{
    public class Workpackage
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Name { get; private set; }
        public void Apply(Event<WorkpackageCreated> created)
        {
            Id = created.Data.Id;
            ProjectId = created.StreamId;
            Name = created.Data.Name;
        }

        public override string ToString()
        {
            return $"Workpackage(Id:{Id}, ProjectId:{ProjectId})";
        }

    }
}
