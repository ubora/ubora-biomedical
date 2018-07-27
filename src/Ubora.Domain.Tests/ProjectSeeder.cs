using System;
using System.Collections.Generic;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Tests
{
    /// <summary>
    /// Test-data builder
    /// </summary>
    public class ProjectSeeder
    {
        private Guid ProjectId { get; set; } = Guid.NewGuid();
        private Guid ProjectCreatorUserId { get; set; } = Guid.NewGuid();
        private List<Guid> MentorUserIds { get; } = new List<Guid>();
        private List<Guid> MemberUserIds { get; } = new List<Guid>();
        private bool? IsWp1Accepted { get; set; }
        private bool? IsWp3Unlocked { get; set; }
        private bool? IsWp4Unlocked { get; set; }

        public ProjectSeeder WithId(Guid projectId)
        {
            ProjectId = projectId;
            return this;
        }

        public ProjectSeeder WithCreator(Guid userId)
        {
            ProjectCreatorUserId = userId;
            return this;
        }

        public ProjectSeeder AddRegularMembers(params Guid[] userIds)
        {
            MemberUserIds.AddRange(userIds);
            return this;
        }

        public ProjectSeeder AddMentors(params Guid[] userIds)
        {
            MentorUserIds.AddRange(userIds);
            return this;
        }

        public ProjectSeeder WithWp1Accepted()
        {
            IsWp1Accepted = true;
            return this;
        }

        public ProjectSeeder WithWp3Unlocked()
        {
            IsWp3Unlocked = true;
            return this;
        }

        public ProjectSeeder WithWp4Unlocked()
        {
            IsWp4Unlocked = true;
            return this;
        }

        public Project Seed(IntegrationFixture fixture)
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

            if (IsWp3Unlocked == true)
            {
                fixture.Processor.Execute(new OpenWorkpackageThreeCommand
                {
                    ProjectId = ProjectId,
                    Actor = new DummyUserInfo()
                }).OnFailure(result => throw new InvalidOperationException(result.ToString()));
            }

            if (IsWp4Unlocked == true)
            {
                fixture.Processor.Execute(new OpenWorkpackageFourCommand
                {
                    ProjectId = ProjectId,
                    Actor = new DummyUserInfo()
                }).OnFailure(result => throw new InvalidOperationException(result.ToString()));
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