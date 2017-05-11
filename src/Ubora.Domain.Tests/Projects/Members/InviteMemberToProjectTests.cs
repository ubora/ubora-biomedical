using FluentAssertions;
using System;
using System.Linq;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public class InviteMemberToProjectTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _invitedUserId = Guid.NewGuid();

        private ICommandResult _lastCommandResult;

        [Fact]
        public void Start()
        {
            this.Given(_ => Given_There_Is_Project_And_User())
                .When(_ => When_User_Is_Invited_To_Project())
                .Then(_ => Then_User_Is_Member_In_Project())
                .When(_ => When_User_Is_Invited_To_Project_Again())
                .Then(_ => Then_User_Is_Not_A_Duplicate_Member_In_Project())
                .BDDfy();
        }

        private void Given_There_Is_Project_And_User()
        {
            Processor.Execute(new CreateProjectCommand
            {
                Id = _projectId,
                UserInfo = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new CreateUserProfileCommand
            {
                UserId = _invitedUserId
            });
        }

        private void When_User_Is_Invited_To_Project()
        {
            // Act
            InviteUserToProject();
        }

        private void InviteUserToProject()
        {
            _lastCommandResult = Processor.Execute(new InviteMemberToProjectCommand
            {
                ProjectId = _projectId,
                UserId = _invitedUserId,
                UserInfo = new UserInfo(Guid.NewGuid(), "")
            });
        }

        private void Then_User_Is_Member_In_Project()
        {
            var project = Session.Load<Project>(_projectId);

            // TODO
            project.DoesSatisfy(new HasMember(_invitedUserId)).Should().BeTrue();

            var addedMember = project.Members.Last();
            addedMember.UserId.Should().Be(_invitedUserId);
            addedMember.Should().BeOfType<ProjectMember>();

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }

        private void When_User_Is_Invited_To_Project_Again()
        {
            InviteUserToProject();
        }

        private void Then_User_Is_Not_A_Duplicate_Member_In_Project()
        {
            var project = Session.Load<Project>(_projectId);
            project.Members.Count(m => m.UserId == _invitedUserId).Should().Be(1);

            _lastCommandResult.IsSuccess.Should().BeFalse();
        }
    }
}
