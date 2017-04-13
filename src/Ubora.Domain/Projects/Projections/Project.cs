using Marten.Events;
using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.Events;

namespace Ubora.Domain.Projects.Projections
{
    public class Project : ISpecifiable<Project>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        private readonly HashSet<Member> _members = new HashSet<Member>();
        public IReadOnlyCollection<Member> Members => _members;

        private void Apply(Event<ProjectCreated> created)
        {
            Name = created.Data.Name;

            var userId = created.Data.Creator.UserId;
            var leader = new Leader(userId);
            _members.Add(leader);
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
