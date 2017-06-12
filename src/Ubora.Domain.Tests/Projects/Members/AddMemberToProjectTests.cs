using FluentAssertions;
using System;
using System.Linq;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public class AddMemberToProjectTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _invitedUserId = Guid.NewGuid();
        private readonly string _invitedUserEmail = "email";

        private ICommandResult _lastCommandResult;

        [Fact]
        public void Non_Existent_Email_Is_Invited_To_Project()
        {
            this.Given(_ => Given_There_Is_Project_And_User())
                .When(_ => When_Non_Existent_Email_Is_Written())
                .Then(_ => Then_Invite_Is_Not_Sent())
                .BDDfy();
        }

        [Fact]
        public void User_Accepts_Invite_To_Project_And_Is_Removed_From_Project()
        {
            this.Given(_ => Given_There_Is_Project_And_User())
                .When(_ => When_User_Is_Invited_To_Project())
                .Then(_ => Then_Invited_User_Has_Invite())
                .When(_ => When_Invited_User_Accepts_Invite())
                .Then(_ => Then_User_Is_Member_In_Project())
                .When(_ => When_User_Is_Invited_To_Project_Again())
                .Then(_ => Then_User_Does_Not_Get_Invite())
                .When(_ => When_Project_Leader_Removes_Invited_User_From_Project())
                .Then(_ => Then_User_Is_Not_Member_In_Project())
                .BDDfy();
        }

        [Fact]
        public void User_Declines_Invite_To_Project()
        {
            this.Given(_ => Given_There_Is_Project_And_User())
                .When(_ => When_User_Is_Invited_To_Project())
                .Then(_ => Then_Invited_User_Has_Invite())
                .When(_ => When_Invited_User_Declines_Invite())
                .Then(_ => Then_User_Is_Not_Member_In_Project())
                .When(_ => When_User_Is_Invited_To_Project_Again())
                .Then(_ => Then_Invited_User_Has_Invite())
                .BDDfy();
        }

        private void When_Project_Leader_Removes_Invited_User_From_Project()
        {
            _lastCommandResult = Processor.Execute(new RemoveMemberFromProjectCommand
            {
                UserId = _invitedUserId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                ProjectId = _projectId
            });
        }

        private void Then_User_Is_Not_Member_In_Project()
        {
            var project = Session.Load<Project>(_projectId);

            project.DoesSatisfy(new HasMember(_invitedUserId)).Should().BeFalse();

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }

        private void When_Invited_User_Declines_Invite()
        {
            var invite = Session.Query<InvitationToProject>().Where(x => x.InvitedMemberId == _invitedUserId).First();

            Processor.Execute(new DeclineMemberInvitationToProjectCommand
            {
                InvitationId = invite.Id,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });
        }

        private void Then_Invite_Is_Not_Sent()
        {
            _lastCommandResult.IsFailure.Should().BeTrue();
        }

        private void When_Non_Existent_Email_Is_Written()
        {
            InviteUserToProject(_invitedUserEmail + Guid.NewGuid());
        }

        private void When_Invited_User_Accepts_Invite()
        {
            var invite = Session.Query<InvitationToProject>().Where(x => x.InvitedMemberId == _invitedUserId).First();

            Processor.Execute(new AcceptMemberInvitationToProjectCommand
            {
                InvitationId = invite.Id,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });
        }

        private void Given_There_Is_Project_And_User()
        {
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = _projectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            Processor.Execute(new CreateUserProfileCommand
            {
                UserId = _invitedUserId,
                Email = _invitedUserEmail
            });
        }

        private void When_User_Is_Invited_To_Project()
        {
            InviteUserToProject(_invitedUserEmail);
        }

        private void InviteUserToProject(string email)
        {
            _lastCommandResult = Processor.Execute(new InviteMemberToProjectCommand
            {
                ProjectId = _projectId,
                InvitedMemberEmail = email,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });
        }

        private void Then_Invited_User_Has_Invite()
        {
            var invites = Session.Query<InvitationToProject>()
                .Where(x => x.InvitedMemberId == _invitedUserId && x.IsAccepted == null);

            invites.Count().Should().Be(1);
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
            InviteUserToProject(_invitedUserEmail);
        }

        private void Then_User_Does_Not_Get_Invite()
        {
            var invites = Session.Query<InvitationToProject>().Where(x => x.InvitedMemberId == _invitedUserId);
            invites.Count().Should().Be(1);
        }
    }
}
