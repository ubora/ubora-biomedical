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
        public void Non_Existent_Email_Is_Not_Invited_To_Project()
        {
            this.Given(_ => There_Is_Project_And_User())
                .When(_ => Non_Existent_Email_Is_Written())
                .Then(_ => Invite_Is_Not_Sent())
                .BDDfy();
        }

        [Fact]
        public void User_Accepts_Invite_To_Project_And_Is_Removed_From_Project()
        {
            this.Given(_ => There_Is_Project_And_User())
                .When(_ => User_Is_Invited_To_Project())
                .Then(_ => Invited_User_Has_Invite())
                .When(_ => Invited_User_Accepts_Invite())
                .Then(_ => User_Is_Member_In_Project())
                .When(_ => User_Is_Invited_To_Project_Again())
                .Then(_ => User_Does_Not_Get_Invite())
                .When(_ => Project_Leader_Removes_Invited_User_From_Project())
                .Then(_ => User_Is_Not_Member_In_Project())
                .BDDfy();
        }

        [Fact]
        public void User_Declines_Invite_To_Project()
        {
            this.Given(_ => There_Is_Project_And_User())
                .When(_ => User_Is_Invited_To_Project())
                .Then(_ => Invited_User_Has_Invite())
                .When(_ => Invited_User_Declines_Invite())
                .Then(_ => User_Is_Not_Member_In_Project())
                .When(_ => User_Is_Invited_To_Project_Again())
                .Then(_ => Invited_User_Has_Invite())
                .BDDfy();
        }

        private void Project_Leader_Removes_Invited_User_From_Project()
        {
            _lastCommandResult = Processor.Execute(new RemoveMemberFromProjectCommand
            {
                UserId = _invitedUserId,
                Actor = new UserInfo(Guid.NewGuid(), ""),
                ProjectId = _projectId
            });
        }

        private void User_Is_Not_Member_In_Project()
        {
            var project = Session.Load<Project>(_projectId);

            project.DoesSatisfy(new HasMember(_invitedUserId)).Should().BeFalse();

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }

        private void Invited_User_Declines_Invite()
        {
            var invite = Session.Query<InvitationToProject>().Where(x => x.InvitedMemberId == _invitedUserId).First();

            Processor.Execute(new DeclineInvitationToProjectCommand
            {
                InvitationId = invite.Id,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });
        }

        private void Invite_Is_Not_Sent()
        {
            _lastCommandResult.IsFailure.Should().BeTrue();
        }

        private void Non_Existent_Email_Is_Written()
        {
            InviteUserToProject(_invitedUserEmail + Guid.NewGuid());
        }

        private void Invited_User_Accepts_Invite()
        {
            var invite = Session.Query<InvitationToProject>().Where(x => x.InvitedMemberId == _invitedUserId).First();

            Processor.Execute(new AcceptInvitationToProjectCommand
            {
                InvitationId = invite.Id,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });
        }

        private void There_Is_Project_And_User()
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

        private void User_Is_Invited_To_Project()
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

        private void Invited_User_Has_Invite()
        {
            var invites = Session.Query<InvitationToProject>()
                .Where(x => x.InvitedMemberId == _invitedUserId && x.IsAccepted == null);

            invites.Count().Should().Be(1);
        }

        private void User_Is_Member_In_Project()
        {
            var project = Session.Load<Project>(_projectId);

            // TODO
            project.DoesSatisfy(new HasMember(_invitedUserId)).Should().BeTrue();

            var addedMember = project.Members.Last();
            addedMember.UserId.Should().Be(_invitedUserId);
            addedMember.Should().BeOfType<ProjectMember>();

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }

        private void User_Is_Invited_To_Project_Again()
        {
            InviteUserToProject(_invitedUserEmail);
        }

        private void User_Does_Not_Get_Invite()
        {
            var invites = Session.Query<InvitationToProject>().Where(x => x.InvitedMemberId == _invitedUserId);
            invites.Count().Should().Be(1);
        }
    }
}
