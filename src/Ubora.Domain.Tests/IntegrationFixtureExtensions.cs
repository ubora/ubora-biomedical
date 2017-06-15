using System;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
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
            fixture.Processor.Execute(new CreateUserProfileCommand
            {
                UserId = userId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Assign_Project_Mentor(this IntegrationFixture fixture, Guid projectId, Guid userId)
        {
            fixture.Processor.Execute(new AssignProjectMentorCommand
            {
                UserId = userId,
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Submit_Workpackage_One_For_Review(this IntegrationFixture fixture, Guid workpackageId)
        {
            fixture.Processor.Execute(new SubmitWorkpackageOneForReviewCommand
            {
                ProjectId = workpackageId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Reject_Workpackage_One_Review(this IntegrationFixture fixture, Guid workpackageId)
        {
            fixture.Processor.Execute(new RejectWorkpackageOneReviewCommand
            {
                ProjectId = workpackageId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Accept_Workpackage_One_Review(this IntegrationFixture fixture, Guid workpackageId)
        {
            fixture.Processor.Execute(new AcceptWorkpackageOneReviewCommand
            {
                ProjectId = workpackageId,
                Actor = new DummyUserInfo()
            });
        }
    }
}