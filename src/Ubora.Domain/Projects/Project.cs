using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects
{
    public class Project : ISpecifiable<Project>
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public string ClinicalNeed { get; set; }
        public string AreaOfUsage { get; set; }
        public string PotentialTechnology { get; set; }
        public string GmdnTerm { get; set; }
        public string GmdnDefinition { get; set; }
        public string GmdnCode { get; set; }

        public WorkpackageOne WorkpackageOne { get; set; } = new WorkpackageOne();

        private readonly HashSet<ProjectMember> _members = new HashSet<ProjectMember>();
        public IReadOnlyCollection<ProjectMember> Members => _members;

        private void Apply(ProjectCreatedEvent e)
        {
            Title = e.Title;
            Description = e.Description;
            AreaOfUsage = e.AreaOfUsage;
            GmdnCode = e.GmdnCode;
            ClinicalNeed = e.ClinicalNeed;
            GmdnDefinition = e.GmdnDefinition;
            GmdnTerm = e.GmdnTerm;
            PotentialTechnology = e.PotentialTechnology;

            var userId = e.Creator.UserId;
            var leader = new ProjectLeader(userId);

            _members.Add(leader);
        }

        private void Apply(ProductSpecificationEditedEvent e)
        {
            WorkpackageOne = new WorkpackageOne
            {
                Functionality = e.Functionality,
                Performance = e.Performance,
                Safety = e.Safety,
                Usability = e.Usability
            };
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

    public class WorkpackageOne
    {
        public string Functionality { get; set; }
        public string Performance { get; set; }
        public string Usability { get; set; }
        public string Safety { get; set; }
    }
}
