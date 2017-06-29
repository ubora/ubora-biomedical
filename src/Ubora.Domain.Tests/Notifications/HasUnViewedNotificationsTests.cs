using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Notifications.Join;
using Ubora.Domain.Notifications.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Notifications
{
    public class HasUnViewedNotificationsTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_UserNotifications_That_Have_Not_Been_Viewed()
        {
            var expectedUserId = Guid.NewGuid();

            var requestToJoinProject = new RequestToJoinProject(Guid.NewGuid(), expectedUserId, Guid.NewGuid(), Guid.NewGuid())
            {
                HasBeenViewed = true
            };
            var expectedRequest = new RequestToJoinProject(Guid.NewGuid(), expectedUserId, Guid.NewGuid(), Guid.NewGuid());

            Session.Store(requestToJoinProject);
            Session.Store(expectedRequest);
            Session.SaveChanges();

            var sut = new HasUnViewedNotifications<RequestToJoinProject>(expectedUserId);
            var invitations = Session.Query<RequestToJoinProject>();

            // Act
            var result = sut.SatisfyEntitiesFrom(invitations);

            // Assert
            var invitation = result.Single();

            invitation.HasBeenViewed.Should().BeFalse();
            invitation.NotificationTo.Should().Be(expectedUserId);
        }
    }
}
