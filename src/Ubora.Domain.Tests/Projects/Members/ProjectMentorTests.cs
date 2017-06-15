using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public static class IntegrationFixtureExtensions
    {
        public static void CreateProject(this IntegrationFixture fixture, Guid projectId)
        {
            fixture.Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new DummyUserInfo()
            });
        }

        public static void CreateProjectWithUser(this IntegrationFixture fixture, Guid projectId, Guid userId)
        {
            fixture.CreateProject(projectId);

            fixture.Processor.Execute(new CreateUserProfileCommand
            {
                UserId = userId,
                Actor = new DummyUserInfo()
            });
        }

        public static void CreateProjectWithMentor(this IntegrationFixture fixture, Guid projectId, Guid userId)
        {
            fixture.CreateProjectWithUser(projectId, userId);

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

        private ICommandResult _lastCommandResult;

        [Fact]
        public void Mentor_Can_Be_Assigned_To_Project()
        {
            this.Given(_ => this.CreateProjectWithUser(_projectId, _userId))
                .When(_ => AssignUserToBeMentorOfProject())
                .Then(_ => AssertUserIsProjectMentor())
                .BDDfy();
        }

        [Fact]
        public void Duplicate_Mentor_Can_Not_Be_Assigned_To_Project()
        {
            this.Given(_ => this.CreateProjectWithMentor(_projectId, _userId))
                .When(_ => AssignUserToBeMentorOfProject())
                .Then(_ => Then_User_Is_Not_Duplicate_Project_Mentor())
                .BDDfy();
        }

        private void AssignUserToBeMentorOfProject()
        {
            _lastCommandResult = Processor.Execute(new AssignProjectMentorCommand
            {
                UserId = _userId,
                ProjectId = _projectId,
                Actor = new DummyUserInfo()
            });
        }

        private void AssertUserIsProjectMentor()
        {
            var project = Session.Load<Project>(_projectId);

            var isUserProjectMentor = project.DoesSatisfy(new HasMember<ProjectMentor>(_userId));

            isUserProjectMentor.Should().BeTrue();
        }


        private void Then_User_Is_Not_Duplicate_Project_Mentor()
        {
            var project = Session.Load<Project>(_projectId);

            project.Members.OfType<ProjectMentor>()
                .Count(m => m.UserId == _userId)
                .Should().Be(1);

            _lastCommandResult.IsFailure
                .Should().BeTrue();
        }
    }
}
