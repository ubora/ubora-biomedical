using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using Marten.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Workpackages.Events;
using Xunit;

namespace Ubora.Domain.Tests.NotifierTests
{
    public class WorkpackageOneSubmittedForReviewEventNotifierTests : IntegrationFixture
    {
        private readonly WorkpackageOneSubmittedForReviewEvent.Notifier _notifierUnderTest;

        public WorkpackageOneSubmittedForReviewEventNotifierTests()
        {
            _notifierUnderTest = Container.Resolve<WorkpackageOneSubmittedForReviewEvent.Notifier>();
        }

        [Fact]
        public void Notifies_Project_Mentors()
        {
            var expectedProjectMentorUserIds = new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
            };

            var project = new ProjectSeeder()
                .AddMentors(expectedProjectMentorUserIds)
                .AddRegularMembers(expectedProjectMentorUserIds[0]) // duplicates should not be notified
                .AddRegularMembers(Guid.NewGuid())
                .Seed(this);

            var eventId = Guid.NewGuid();
            var martenEvent = new Event<WorkpackageOneSubmittedForReviewEvent>(
                data: new WorkpackageOneSubmittedForReviewEvent(
                    initiatedBy: new DummyUserInfo(),
                    projectId: project.Id,
                    reviewId: Guid.NewGuid(), 
                    submittedAt: DateTimeOffset.MinValue))
            {
                Id = eventId
            };

            Session.DeleteWhere<INotification>(_ => true);

            // Act
            _notifierUnderTest.Handle(martenEvent);

            // Assert
            var notifications = Session.Query<INotification>().ToList();
            notifications.Cast<EventNotification>()
                .All(x => x.EventId == eventId && x.EventType == typeof(WorkpackageOneSubmittedForReviewEvent))
                .Should().BeTrue();

            notifications.Count.Should().Be(expectedProjectMentorUserIds.Count());

            notifications.Select(n => n.NotificationTo).Should().BeEquivalentTo(expectedProjectMentorUserIds);
        }
    }
}