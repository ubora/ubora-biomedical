using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Events;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Projects._Specifications;

namespace Ubora.Domain.Projects
{
    public class Project : Entity<Project>
    {
        public Guid Id { get; private set; }

        public bool HasMarkdownBeenConvertedToQuillDelta { get; set; }

        public string Title { get; private set; }
        public string Gmdn { get; private set; }
        public string ClinicalNeedTags { get; private set; }
        public string AreaOfUsageTags { get; private set; }
        public string PotentialTechnologyTags { get; private set; }
        public string Description { get; private set; }
        public bool IsInDraft { get; private set; } = true;
        public BlobLocation ProjectImageBlobLocation { get; private set; }
        public DateTime ProjectImageLastUpdated { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedDateTime { get; set; }

        [JsonIgnore]
        public bool HasImage => new HasImageSpec().IsSatisfiedBy(this);

        [JsonProperty(nameof(Projects.Members))]
        private HashSet<ProjectMember> _members = new HashSet<ProjectMember>();

        [JsonIgnore]
        // Virtual for testing.
        public virtual IReadOnlyCollection<ProjectMember> Members
        {
            get { return _members.ToList().AsReadOnly(); }
            private set { _members = value.ToHashSet(); }
        }

        public virtual IReadOnlyCollection<ProjectMember> GetMembers(ISpecification<ProjectMember> spec = null)
        {
            spec = spec ?? new MatchAll<ProjectMember>();
            return spec.SatisfyEntitiesFrom(Members).ToList().AsReadOnly();
        }
        
        public bool HasMember<T>(Guid userId) where T : ProjectMember
        {
            return DoesSatisfy(new HasMember<T>(userId));
        }

        private void Apply(WorkpackageOneStepEditedEventV2 @event)
        {
            HasMarkdownBeenConvertedToQuillDelta = true;
        }

        private void Apply(ProjectCreatedEvent e)
        {
            Id = e.ProjectId;
            Title = e.Title;
            AreaOfUsageTags = e.AreaOfUsage;
            ClinicalNeedTags = e.ClinicalNeed;
            Gmdn = e.Gmdn;
            PotentialTechnologyTags = e.PotentialTechnology;
            CreatedDateTime = e.Timestamp.UtcDateTime;

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

        private void Apply(MemberRemovedFromProjectEvent e)
        {
            var doesNotHaveMember = this.DoesSatisfy(!new HasMember<ProjectMember>(e.UserId));
            if (doesNotHaveMember)
            {
                throw new InvalidOperationException();
            }

            _members.RemoveWhere(m => m.UserId == e.UserId);
        }

        private void Apply(ProjectLeaderPromotedEvent e)
        {
            var doesNotHaveMember = this.DoesSatisfy(!new HasMember<ProjectMember>(e.UserId));
            if (doesNotHaveMember)
            {
                throw new InvalidOperationException();
            }

            var member = _members.FirstOrDefault(m => m.IsLeader);
            _members.Remove(member);
            _members.Add(new ProjectMember(member.UserId));
            _members.Add(new ProjectLeader(e.UserId));
        }

        private void Apply(EditProjectDescriptionEvent e)
        {
            Description = e.Description;
        }

        private void Apply(ProjectTitleEditedEvent e)
        {
            Title = e.Title;
        }

        private void Apply(WorkpackageOneReviewAcceptedEvent e)
        {
            IsInDraft = false;
        }

        private void Apply(WorkpackageOneReopenedAfterAcceptanceByReviewEvent e)
        {
            IsInDraft = true;
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

        private void Apply(MentorJoinedProjectEvent e)
        {
            var isMentorAlready = this.DoesSatisfy(new HasMember<ProjectMentor>(e.UserId));
            if (isMentorAlready)
            {
                throw new InvalidOperationException();
            }

            _members.Add(new ProjectMentor(e.UserId));
        }

        private void Apply(ProjectDeletedEvent e)
        {
            if (IsDeleted) { throw new InvalidOperationException(); }
            IsDeleted = true;
        }

        public override string ToString()
        {
            return $"Project(Id:{Id})";
        }
    }
}
