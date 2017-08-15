using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects.Members;

namespace Ubora.Domain.Tests.Notifications
{
    public class HasPendingNotificationsTests : IntegrationFixture
    {
        [Fact]
        public void Returns_Notifications_That_Have_Been_Viewed()
        {
            var expectedUserId = Guid.NewGuid();

            var invitationToProject = new InvitationToProject(expectedUserId, Guid.NewGuid());
            var expectedInvitation = new InvitationToProject(expectedUserId, Guid.NewGuid());
            expectedInvitation.Accept();

            Session.Store(invitationToProject);
            Session.Store(expectedInvitation);
            Session.SaveChanges();

            var sut = new HasPendingNotifications<InvitationToProject>(expectedUserId);
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
