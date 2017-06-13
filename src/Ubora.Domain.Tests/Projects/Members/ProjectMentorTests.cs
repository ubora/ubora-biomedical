using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.WorkpackageTwos;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public static class IntegrationFixtureExtensions
    {
        public static void Given_There_Is_Project(this IntegrationFixture fixture, Guid projectId)
        {
            fixture.Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Given_There_Is_Project_And_User(this IntegrationFixture fixture, Guid projectId, Guid userId)
        {
            fixture.Given_There_Is_Project(projectId);

            fixture.Processor.Execute(new CreateUserProfileCommand
            {
                UserId = userId,
                Actor = new DummyUserInfo()
            });
        }

        public static void Given_There_Is_Project_With_Mentor(this IntegrationFixture fixture, Guid projectId, Guid userId)
        {
            fixture.Given_There_Is_Project_And_User(projectId, userId);

            fixture.Processor.Execute(new AssignProjectMentorCommand
            {
                UserId = userId,
                ProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }
    }

    public class ProjectMentorTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();

        [Fact]
        public void Mentor_Can_Be_Assigned_To_Project()
        {
            this.Given(_ => this.Given_There_Is_Project_And_User(_projectId, _userId))
                .When(_ => When_User_Is_Assigned_To_Be_Mentor_Of_Project())
                .Then(_ => Then_User_Is_Project_Mentor())
                .BDDfy();
        }

        private void When_User_Is_Assigned_To_Be_Mentor_Of_Project()
        {
            Processor.Execute(new AssignProjectMentorCommand
            {
                UserId = _userId,
                ProjectId = _projectId,
                Actor = new DummyUserInfo()
            });
        }

        private void Then_User_Is_Project_Mentor()
        {
            var project = Session.Load<Project>(_projectId);

            var isUserProjectMentor = project.DoesSatisfy(new HasMember<ProjectMentor>(_userId));

            isUserProjectMentor.Should().BeTrue();
        }

        [Fact]
        public void Duplicate_Mentor_Can_Not_Be_Assigned_To_Project()
        {
            this.Given(_ => this.Given_There_Is_Project_With_Mentor(_projectId, _userId))
                .When(_ => When_User_Is_Assigned_To_Be_Mentor_Of_Project())
                .Then(_ => Then_User_Is_Not_Duplicate_Project_Mentor())
                .BDDfy();
        }

        private void Then_User_Is_Not_Duplicate_Project_Mentor()
        {
            var project = Session.Load<Project>(_projectId);

            project.Members.OfType<ProjectMentor>()
                .Count(m => m.UserId == _userId)
                .Should().Be(1);
        }
    }
}
