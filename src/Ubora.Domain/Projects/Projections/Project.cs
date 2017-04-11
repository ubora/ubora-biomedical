using Marten.Events;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModels.Specifications;
using Ubora.Domain.Projects.Events;
using Ubora.Domain.Specifications;

namespace Ubora.Domain.Projects.Projections
{
    public class Project : ISpecifiable<Project>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        private void Apply(Event<ProjectCreated> created)
        {
            Name = created.Data.Name;
        }

        private void Apply(Event<ProjectRenamed> renamed)
        {
            Name = renamed.Data.NewName;
        }

        public override string ToString()
        {
            return $"Project(Id:{Id})";
        }

        public bool DoesSatisfy(ISpecification<Project> specification)
        {
            return specification.IsSatisfiedBy(this);
        }
    }
}
