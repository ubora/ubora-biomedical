using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects._Commands;
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
        private Guid? RelatedClinicalNeed { get; set; }
        private List<Guid> MentorUserIds { get; } = new List<Guid>();
        private List<Guid> MemberUserIds { get; } = new List<Guid>();
        private bool? IsWp1Accepted { get; set; }
        private bool? IsWp3Unlocked { get; set; }
        private bool? IsWp4Unlocked { get; set; }
        private bool? IsWp5Unlocked { get; set; }
        private bool? IsWp6Unlocked { get; set; }
        private bool IsDeleted { get; set; }

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

        public ProjectSeeder WithWp5Unlocked()
        {
            IsWp5Unlocked = true;
            return this;
        }

        public ProjectSeeder WithWp6Unlocked()
        {
            IsWp6Unlocked = true;
            return this;
        }

        public ProjectSeeder WithRelatedClinicalNeed(Guid clinicalNeedId)
        {
            RelatedClinicalNeed = clinicalNeedId;
            return this;
        }

        public ProjectSeeder AsDeleted()
        {
            IsDeleted = true;
            return this;
        }

        public Project Seed(IntegrationFixture fixture)
        {
            CreateUserProfileIfNecessary(ProjectCreatorUserId, fixture);

            fixture.Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = ProjectId,
                RelatedClinicalNeedId = RelatedClinicalNeed,
                Actor = new UserInfo(ProjectCreatorUserId, "Mr. Project Leader")
            });

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

            if (IsWp5Unlocked == true)
            {
                fixture.Processor.Execute(new OpenWorkpackageFiveCommand
                {
                    ProjectId = ProjectId,
                    Actor = new DummyUserInfo()
                }).OnFailure(result => throw new InvalidOperationException(result.ToString()));
            }

            if (IsWp6Unlocked == true)
            {
                fixture.Processor.Execute(new OpenWorkpackageSixCommand
                {
                    ProjectId = ProjectId,
                    Actor = new DummyUserInfo()
                }).OnFailure(result => throw new InvalidOperationException(result.ToString()));
            }

            if (IsDeleted)
            {
                fixture.Processor.Execute(new DeleteProjectCommand
                {
                    ProjectId = ProjectId,
                    Actor = new DummyUserInfo()
                });
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