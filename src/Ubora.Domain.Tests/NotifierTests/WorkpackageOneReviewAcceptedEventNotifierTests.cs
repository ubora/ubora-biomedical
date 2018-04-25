﻿using System;
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
    public class WorkpackageOneReviewAcceptedEventNotifierTests : IntegrationFixture
    {
        private readonly WorkpackageOneReviewAcceptedEvent.Notifier _notifierUnderTest;

        public WorkpackageOneReviewAcceptedEventNotifierTests()
        {
            _notifierUnderTest = Container.Resolve<WorkpackageOneReviewAcceptedEvent.Notifier>();
        }

        [Fact]
        public void Notifies_Project_Members_Except_Event_Invoker()
        {
            var eventInvokerUserId = Guid.NewGuid();
            var expectedNotifiedProjectMemberUserIds = new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };

            var project = new ProjectBuilder()
                .WithCreator(expectedNotifiedProjectMemberUserIds[0])
                .AddMentors(expectedNotifiedProjectMemberUserIds[1], eventInvokerUserId)
                .AddRegularMembers(expectedNotifiedProjectMemberUserIds[2])
                .AddRegularMembers(expectedNotifiedProjectMemberUserIds[2]) // duplicates should not be notified
                .Build(this);

            var eventId = Guid.NewGuid();
            var martenEvent = new Event<WorkpackageOneReviewAcceptedEvent>(
                data: new WorkpackageOneReviewAcceptedEvent(
                    initiatedBy: new UserInfo(eventInvokerUserId, ""),
                    projectId: project.Id,
                    concludingComment: "whatever",
                    acceptedAt: DateTimeOffset.MinValue))
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
                .All(x => x.EventId == eventId && x.EventType == typeof(WorkpackageOneReviewAcceptedEvent))
                .Should().BeTrue();

            notifications.Count.Should().Be(expectedNotifiedProjectMemberUserIds.Count());

            notifications.Select(n => n.NotificationTo).Should().BeEquivalentTo(expectedNotifiedProjectMemberUserIds);
        }
    }
}