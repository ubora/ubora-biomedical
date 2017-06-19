using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Notifications
{
    public class UserInvitationsTests: IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_UserInvitations()
        {
            var userId = Guid.NewGuid();
            var expectedUserId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateUserProfileCommand
            {
                UserId = userId,
                Email = "foo@goo.com",
                Actor = new UserInfo(userId, "")
            });
            Processor.Execute(new CreateUserProfileCommand
            {
                UserId = expectedUserId,
                Email = "jane@doe.com",
                Actor = new UserInfo(userId, "")
            });

            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Title = "title",
                Actor = new UserInfo(userId, "")
            });

            Processor.Execute(new InviteMemberToProjectCommand
            {
                ProjectId = projectId,
                InvitedMemberEmail = "jane@doe.com",
                Actor = new UserInfo(userId, "")
            });

            var sut = new UserInvitations(expectedUserId);
            var invitations = Session.Query<InvitationToProject>();

            // Act
            var result = sut.SatisfyEntitiesFrom(invitations);

            // Assert
            var invitation = result.Single();

            invitation.HasBeenViewed.Should().BeFalse();
            invitation.InvitedMemberId.Should().Be(expectedUserId);
        }
    }
}
