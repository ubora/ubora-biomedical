﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.DeviceClassification;
using System.Linq;

namespace Ubora.Domain.Projects
{
    public class Project : Entity<Project>
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }

        public string Gmdn { get; private set; }
        public string ClinicalNeedTags { get; private set; }
        public string AreaOfUsageTags { get; private set; }
        public string PotentialTechnologyTags { get; private set; }
        public string DeviceClassification { get; private set; }
        public string Description { get; set; }

        [JsonProperty(nameof(Members))]
        private readonly HashSet<ProjectMember> _members = new HashSet<ProjectMember>();
        [JsonIgnore]
        public IReadOnlyCollection<ProjectMember> Members => _members;

        private void Apply(ProjectCreatedEvent e)
        {
            Id = e.Id;
            Title = e.Title;
            AreaOfUsageTags = e.AreaOfUsage;
            ClinicalNeedTags = e.ClinicalNeed;
            Gmdn = e.Gmdn;
            PotentialTechnologyTags = e.PotentialTechnology;

            var userId = e.InitiatedBy.UserId;
            var leader = new ProjectLeader(userId);

            _members.Add(leader);
        }

        private void Apply(ProjectUpdatedEvent e)
        {
            Title = e.Title;
            ClinicalNeedTags = e.ClinicalNeedTags;
            AreaOfUsageTags = e.AreaOfUsageTags;
            PotentialTechnologyTags = e.PotentialTechnologyTags;
            Gmdn = e.Gmdn;
        }

        private void Apply(MemberAddedToProjectEvent e)
        {
            var alreadyHasMember = this.DoesSatisfy(new HasMember<ProjectMember>(e.UserId));
            if (alreadyHasMember)
            {
                throw new InvalidOperationException();
            }

            var member = new ProjectMember(e.UserId);
            _members.Add(member);
        }

        private void Apply(MemberRemovedFromProjectEvent e)
        {
            var doesNotHaveMember = this.DoesSatisfy(!new HasMember<ProjectMember>(e.UserId));
            if (doesNotHaveMember)
            {
                throw new InvalidOperationException();
            }

            var member = _members.Single(x => x.UserId == e.UserId);
            _members.Remove(member);
        }

        private void Apply(DeviceClassificationSetEvent e)
        {
            DeviceClassification = e.DeviceClassification;
        }

        private void Apply(EditProjectDescriptionEvent e)
        {
            Description = e.Description;
        }

        private void Apply(ProjectMentorAssignedEvent e)
        {
            var isAlreadyMentor = this.DoesSatisfy(new HasMember<ProjectMentor>(e.UserId));
            if (isAlreadyMentor)
            {
                return;
            }
            _members.Add(new ProjectMentor(e.UserId));
        }

        public override string ToString()
        {
            return $"Project(Id:{Id})";
        }
    }
}
