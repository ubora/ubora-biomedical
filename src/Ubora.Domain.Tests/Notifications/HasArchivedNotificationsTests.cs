using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications.Invitation;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;
using Xunit;
using Ubora.Domain.Notifications;
using System.Linq;
using FluentAssertions;

namespace Ubora.Domain.Tests.Notifications
{
    public class HasArchivedNotificationsTests : IntegrationFixture
    {
        [Fact]
        public void Returns_Notifications_That_Have_Been_Viewed()
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

            var invite = Session.Query<InvitationToProject>().Single();

            Processor.Execute(new DeclineInvitationToProjectCommand
            {
                InvitationId = invite.Id,
                Actor = new UserInfo(userId, "")
            });

            var sut = new HasArchivedNotifications<InvitationToProject>(expectedUserId);
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
