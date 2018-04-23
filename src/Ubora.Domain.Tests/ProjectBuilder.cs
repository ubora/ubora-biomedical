using System;
using System.Collections.Generic;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

namespace Ubora.Domain.Tests
{
    /// <summary>
    /// Test-data builder
    /// </summary>
    public class ProjectBuilder
    {
        private Guid ProjectId { get; set; } = Guid.NewGuid();
        private Guid ProjectCreatorUserId { get; set; } = Guid.NewGuid();
        private List<Guid> MentorUserIds { get; } = new List<Guid>();
        private List<Guid> MemberUserIds { get; } = new List<Guid>();
        private bool? IsWp1Accepted { get; set; }

        public ProjectBuilder WithId(Guid projectId)
        {
            ProjectId = projectId;
            return this;
        }

        public ProjectBuilder WithCreator(Guid userId)
        {
            ProjectCreatorUserId = userId;
            return this;
        }

        public ProjectBuilder AddRegularMembers(params Guid[] userIds)
        {
            MemberUserIds.AddRange(userIds);
            return this;
        }

        public ProjectBuilder AddMentors(params Guid[] userIds)
        {
            MentorUserIds.AddRange(userIds);
            return this;
        }

        public ProjectBuilder WithWp1Accepted()
        {
            IsWp1Accepted = true;
            return this;
        }

        public Project Build(IntegrationFixture fixture)
        {
            CreateUserProfileIfNecessary(ProjectCreatorUserId, fixture);
            fixture.Create_Project(ProjectId, ProjectCreatorUserId);

            foreach (var memberUserId in MemberUserIds)
            {
                CreateUserProfileIfNecessary(memberUserId, fixture);
                fixture.Add_Project_Member(ProjectId, memberUserId);
            }

            foreach (var mentorUserId in MentorUserIds)
            {
                CreateUserProfileIfNecessary(mentorUserId, fixture);
                fixture.Assign_Project_Mentor(ProjectId, mentorUserId);
            }

            if (IsWp1Accepted == true)
            {
                fixture.Submit_Workpackage_One_For_Review(ProjectId);
                fixture.Accept_Workpackage_One_Review(ProjectId);
            }

            return fixture.Processor.FindById<Project>(ProjectId);
        }

        private void CreateUserProfileIfNecessary(Guid userId, IntegrationFixture fixture)
        {
            var userProfile = fixture.Processor.FindById<UserProfile>(userId);
            if (userProfile == null)
            {
                fixture.Create_User(userId);
            }
        }
    }
}