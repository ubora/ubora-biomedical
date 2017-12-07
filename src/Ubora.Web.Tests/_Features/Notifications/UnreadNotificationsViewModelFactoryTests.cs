using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Notifications;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Queries;
using Ubora.Web._Features.Notifications;
using Xunit;

namespace Ubora.Web.Tests._Features.Notifications
{
    public class UnreadNotificationsViewModelFactoryTests
    {
        private readonly UnreadNotificationsViewModel.Factory _unreadNotificationsViewModelFactory;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;

        public UnreadNotificationsViewModelFactoryTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _unreadNotificationsViewModelFactory = new UnreadNotificationsViewModel.Factory(_queryProcessorMock.Object);
        }

        [Fact]
        public void Create_Creates_ViewModel()
        {
            var userId = Guid.NewGuid();
      
            _queryProcessorMock.Setup(x => x.ExecuteQuery(It.Is<CountQuery<INotification>>(c => (Specification<INotification>)c.Specification == new HasUnViewedNotifications(userId))))
                .Returns(3);

            // Act
            var result = _unreadNotificationsViewModelFactory.Create(userId);

            // Assert
            result.UnreadMessagesCount.Should().Be(3);
        }
    }
}
