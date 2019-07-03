using Autofac;
using FluentAssertions;
using Marten.Events;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects._Events;
using Xunit;

namespace Ubora.Domain.Tests.NotifierTests
{
    public class MentorJoinedProjectEventNotifierTests : IntegrationFixture
    {
        private readonly MentorJoinedProjectEvent.Notifier _notifierUnderTest;

        public MentorJoinedProjectEventNotifierTests()
        {
            _notifierUnderTest = Container.Resolve<MentorJoinedProjectEvent.Notifier>();
        }

        [Fact]
        public void Notifies_Project_Leader()
        {
            var eventInvokerUserId = Guid.NewGuid();
            var expectedNotifiedProjectMemberUserIds = new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };

            var project = new ProjectSeeder()
                .WithCreator(expectedNotifiedProjectMemberUserIds[0])
                .AddMentors(expectedNotifiedProjectMemberUserIds[1], eventInvokerUserId)
                .AddRegularMembers(expectedNotifiedProjectMemberUserIds[2])
                .AddRegularMembers(expectedNotifiedProjectMemberUserIds[2]) // duplicates should not be notified
                .Seed(this);

            var eventId = Guid.NewGuid();
            var martenEvent = new Event<MentorJoinedProjectEvent>(
                data: new MentorJoinedProjectEvent(
                    initiatedBy: new UserInfo(eventInvokerUserId, ""),
                    projectId: project.Id,
                    userId: Guid.NewGuid()))
            {
                Id = eventId
            };

            Session.DeleteWhere<INotification>(_ => true);

            // Act
            _notifierUnderTest.Handle(martenEvent);

            // Assert
            RefreshSession();

            var notifications = Session.Query<INotification>().ToList();
            notifications.Cast<EventNotification>()
                .All(x => x.EventId == eventId && x.EventType == typeof(MentorJoinedProjectEvent))
                .Should().BeTrue();

            notifications.Count.Should().Be(1);
            notifications.Select(n => n.NotificationTo).Should().BeEquivalentTo(expectedNotifiedProjectMemberUserIds[0]);
        }
    }
}
