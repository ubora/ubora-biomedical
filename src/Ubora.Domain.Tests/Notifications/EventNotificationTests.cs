using System;
using FluentAssertions;
using Marten.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects._Events;
using Xunit;

namespace Ubora.Domain.Tests.Notifications
{
    public class EventNotificationTests
    {
        [Fact]
        public void Create_Creates_New_EventNotification()
        {
            var @event = new ProjectDeletedEvent(new DummyUserInfo(), Guid.NewGuid());
            var eventWithMetadata = new Event<ProjectDeletedEvent>(@event).Set(e => e.Id, Guid.NewGuid());
            var userId = Guid.NewGuid();

            // Act
            var result = EventNotification.Create(@event, eventWithMetadata.Id, userId);

            // Assert
            result.Id.Should().NotBe(default(Guid));
            result.EventId.Should().Be(eventWithMetadata.Id);
            result.EventType.Should().Be(typeof(ProjectDeletedEvent));
        }
    }
}
