using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects.Members;
using Xunit;

namespace Ubora.Domain.Tests.Notifications
{
    public class IsForUserTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_UserNotifications()
        {
            var expectedUserId = Guid.NewGuid();

            var requestToJoinProject = new InvitationToProject(Guid.NewGuid(), Guid.NewGuid());
            var expectedRequest = new InvitationToProject(expectedUserId, Guid.NewGuid());

            Session.Store(requestToJoinProject);
            Session.Store(expectedRequest);
            Session.SaveChanges();

            var sut = new IsForUser<InvitationToProject>(expectedUserId);
            var invitations = Session.Query<InvitationToProject>();

            // Act
            var result = sut.SatisfyEntitiesFrom(invitations);

            // Assert
            var invitation = result.Single();

            invitation.HasBeenViewed.Should().BeFalse();
            invitation.NotificationTo.Should().Be(expectedUserId);
        }
    }
}
