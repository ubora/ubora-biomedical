using System;
using System.Linq;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Tests
{
    public static class IntegrationFixtureExtensions
    {
        public static void Create_Project(this IntegrationFixture fixture, Guid projectId)
        {
            fixture.Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Create_User(this IntegrationFixture fixture, Guid userId)
        {
            fixture.Create_User(userId, email: null);
        }

        public static void Create_User(this IntegrationFixture fixture, Guid userId, string email)
        {
            fixture.Processor.Execute(new CreateUserProfileCommand
            {
                UserId = userId,
                Email = email,
                Actor = new DummyUserInfo()
            });
        }

        public static void Assign_Project_Mentor(this IntegrationFixture fixture, Guid projectId, Guid userId)
        {
            fixture.Processor.Execute(new InviteProjectMentorCommand
            {
                UserId = userId,
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            });

            var invitation = fixture.Processor.Find<ProjectMentorInvitation>()
                .Last(x => x.InviteeUserId == userId);

            fixture.Processor.Execute(new AcceptProjectMentorInvitationCommand
            {
                Actor = new DummyUserInfo(),
                InvitationId = invitation.Id
            });
        }

        public static void Submit_Workpackage_One_For_Review(this IntegrationFixture fixture, Guid projectId)
        {
            fixture.Processor.Execute(new SubmitWorkpackageOneForReviewCommand
            {
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Reject_Workpackage_One_Review(this IntegrationFixture fixture, Guid projectId)
        {
            fixture.Processor.Execute(new RejectWorkpackageOneReviewCommand
            {
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Accept_Workpackage_One_Review(this IntegrationFixture fixture, Guid projectId)
        {
            fixture.Processor.Execute(new AcceptWorkpackageOneReviewCommand
            {
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Submit_Workpackage_Two_For_Review(this IntegrationFixture fixture, Guid projectId)
        {
            fixture.Processor.Execute(new SubmitWorkpackageTwoForReviewCommand
            {
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Reject_Workpackage_Two_Review(this IntegrationFixture fixture, Guid projectId)
        {
            fixture.Processor.Execute(new RejectWorkpackageTwoReviewCommand
            {
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Accept_Workpackage_Two_Review(this IntegrationFixture fixture, Guid projectId)
        {
            fixture.Processor.Execute(new AcceptWorkpackageTwoReviewCommand
            {
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }
    }
}