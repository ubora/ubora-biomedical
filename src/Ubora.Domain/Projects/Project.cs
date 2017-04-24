using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects
{
    public class Project : ISpecifiable<Project>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        private readonly HashSet<ProjectMember> _members = new HashSet<ProjectMember>();
        public IReadOnlyCollection<ProjectMember> Members => _members;

        private void Apply(ProjectCreatedEvent @event)
        {
            Name = @event.Name;

            var userId = @event.Creator.UserId;
            var leader = new ProjectLeader(userId);

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
