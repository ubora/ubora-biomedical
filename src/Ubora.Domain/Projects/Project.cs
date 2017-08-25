﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects
{
    public class Project : Entity<Project>
    {
        // Virtual for testing
        public virtual Guid Id { get; private set; }
        public virtual string Title { get; private set; }
        public string Gmdn { get; private set; }
        public string ClinicalNeedTags { get; private set; }
        public string AreaOfUsageTags { get; private set; }
        public string PotentialTechnologyTags { get; private set; }
        public string DeviceClassification { get; private set; }
        public string Description { get; private set; }
        public bool IsInDraft { get; private set; } = true;
        public BlobLocation ProjectImageBlobLocation { get; set; }
        public DateTime ProjectImageLastUpdated { get; private set; }
        public bool HasImage => ProjectImageBlobLocation != null;

        [JsonProperty(nameof(Members))]
        private readonly HashSet<ProjectMember> _members = new HashSet<ProjectMember>();
        [JsonIgnore]
        public IReadOnlyCollection<ProjectMember> Members
        {
            get
            {
                return _members;
            }
            private set { }
        }

        public bool HasMember<T>(Guid userId) where T : ProjectMember
        {
            return DoesSatisfy(new HasMember<T>(userId));
        }

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

        private void Apply(MemberAcceptedToJoinProjectEvent e)
        {
            var member = new ProjectMember(e.UserId);
            _members.Add(member);
        }

        private void Apply(EditedProjectDeviceClassificationEvent e)
        {
            if (e.CurrentClassification == null || e.NewClassification > e.CurrentClassification)
            {
                DeviceClassification = e.NewClassification.Text;
            }
        }

        private void Apply(MemberRemovedFromProjectEvent e)
        {
            var doesNotHaveMember = this.DoesSatisfy(!new HasMember<ProjectMember>(e.UserId));
            if (doesNotHaveMember)
            {
                throw new InvalidOperationException();
            }

            _members.RemoveWhere(m => m.UserId == e.UserId);
        }

        private void Apply(EditProjectDescriptionEvent e)
        {
            Description = e.Description;
        }

        private void Apply(ProjectMentorAssignedEvent e)
        {
            var isMember = this.DoesSatisfy(new HasMember<ProjectMember>(e.UserId));
            if (isMember)
            {
                _members.RemoveWhere(m => m.UserId == e.UserId);
            }

            var isAlreadyMentor = this.DoesSatisfy(new HasMember<ProjectMentor>(e.UserId));
            if (isAlreadyMentor)
            {
                return;
            }
            _members.Add(new ProjectMentor(e.UserId));
        }

        private void Apply(WorkpackageOneReviewAcceptedEvent e)
        {
            IsInDraft = false;
        }

        private void Apply(ProjectImageUpdatedEvent e)
        {
            ProjectImageBlobLocation = e.BlobLocation;
            ProjectImageLastUpdated = e.When;
        }

        private void Apply(ProjectImageDeletedEvent e)
        {
            ProjectImageBlobLocation = null;
            ProjectImageLastUpdated = e.When;
        }

        public override string ToString()
        {
            return $"Project(Id:{Id})";
        }
    }
}
