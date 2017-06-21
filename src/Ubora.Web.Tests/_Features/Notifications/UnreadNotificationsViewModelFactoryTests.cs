using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Notifications.Invitation;
using Ubora.Web._Features.Notifications;
using Xunit;

namespace Ubora.Web.Tests._Features.Notifications
{
    public class UnreadNotificationsViewModelFactoryTests
    {
        private UnreadNotificationsViewModel.Factory _unreadNotificationsViewModelFactory;
        private Mock<IQueryProcessor> _queryProcessorMock;

        public UnreadNotificationsViewModelFactoryTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _unreadNotificationsViewModelFactory = new UnreadNotificationsViewModel.Factory(_queryProcessorMock.Object);
        }

        [Fact]
        public void Create_Creates_ViewModel()
        {
            var userId = Guid.NewGuid();

            var invitation1 = new InvitationToProject(Guid.NewGuid(), userId, userId, Guid.NewGuid());
            var invitation2 = new InvitationToProject(Guid.NewGuid(), userId, userId, Guid.NewGuid());
            var invitation3 = new InvitationToProject(Guid.NewGuid(), userId, userId, Guid.NewGuid());

            var invitations = new List<InvitationToProject>
            {
                invitation1,
                invitation2,
                invitation3
            };

            _queryProcessorMock.Setup(x => x.Find(It.IsAny<NonViewedNotifications>()))
                .Returns(invitations);

            // Act
            var result = _unreadNotificationsViewModelFactory.Create(userId);

            // Assert
            result.UnreadMessagesCount.Should().Be(3);
        }
    }
}
