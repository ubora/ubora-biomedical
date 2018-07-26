using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Workpackages.Events;
using Xunit;

namespace Ubora.Domain.Tests.NotifierTests
{
    public class WorkpackageOneReopenedAfterAcceptanceByReviewEventNotifierTests : IntegrationFixture
    {
        private readonly WorkpackageOneReopenedAfterAcceptanceByReviewEvent.Notifier _notifierUnderTest;

        public WorkpackageOneReopenedAfterAcceptanceByReviewEventNotifierTests()
        {
            _notifierUnderTest = Container.Resolve<WorkpackageOneReopenedAfterAcceptanceByReviewEvent.Notifier>();
        }

        [Fact]
        public void Notifies_Project_Members_Except_Event_Invoker()
        {
            var eventInvokerUserId = Guid.NewGuid();
            var expectedProjectMemberUserIds = new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };

            var project = new ProjectSeeder()
                .WithCreator(expectedProjectMemberUserIds[0])
                .AddMentors(expectedProjectMemberUserIds[1], eventInvokerUserId)
                .AddRegularMembers(expectedProjectMemberUserIds[2])
                .Seed(this);

            var eventId = Guid.NewGuid();
            var martenEvent = new Event<WorkpackageOneReopenedAfterAcceptanceByReviewEvent>(
                data: new WorkpackageOneReopenedAfterAcceptanceByReviewEvent(
                    initiatedBy: new UserInfo(eventInvokerUserId, ""),
                    projectId: project.Id,
                    acceptedReviewId: Guid.NewGuid()))
            {
                Id = eventId
            };

            Session.DeleteWhere<INotification>(_ => true);

            // Act
            _notifierUnderTest.Handle(martenEvent);

            // Assert
            var notifications = Session.Query<INotification>().ToList();
            notifications.Cast<EventNotification>()
                .All(x => x.EventId == eventId && x.EventType == typeof(WorkpackageOneReopenedAfterAcceptanceByReviewEvent))
                .Should().BeTrue();

            notifications.Count.Should().Be(expectedProjectMemberUserIds.Count());

            notifications.Select(n => n.NotificationTo).Should().BeEquivalentTo(expectedProjectMemberUserIds);
        }
    }
}