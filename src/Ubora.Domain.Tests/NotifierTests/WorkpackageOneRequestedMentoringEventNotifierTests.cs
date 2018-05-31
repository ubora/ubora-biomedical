using Autofac;
using FluentAssertions;
using Marten.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;
using Xunit;

namespace Ubora.Domain.Tests.NotifierTests
{
    public class WorkpackageOneRequestedMentoringEventNotifierTests : IntegrationFixture
    {
        private readonly WorkpackageOneReviewRequestedMentoringEvent.Notifier _notifierUnderTest;
        private readonly TestFindUboraAdministratorsQueryHandler _findUboraAdministratorsQueryHandler = new TestFindUboraAdministratorsQueryHandler();

        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            builder.RegisterInstance(_findUboraAdministratorsQueryHandler)
                .As<IQueryHandler<FindUboraAdministratorsQuery, IReadOnlyCollection<UserProfile>>>()
                .SingleInstance();
        }

        public WorkpackageOneRequestedMentoringEventNotifierTests()
        {
            _notifierUnderTest = Container.Resolve<WorkpackageOneReviewRequestedMentoringEvent.Notifier>();
        }

        [Fact]
        public void Notifies_Administrators()
        {
            var administrators = new List<UserProfile> { new UserProfile(Guid.NewGuid()), new UserProfile(Guid.NewGuid()) };
            _findUboraAdministratorsQueryHandler.AdministratorsProfilesToReturn = administrators;

            var eventId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var martenEvent = new Event<WorkpackageOneReviewRequestedMentoringEvent>(
                data: new WorkpackageOneReviewRequestedMentoringEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: projectId))
            {
                Id = eventId
            };

            Session.DeleteWhere<INotification>(_ => true);

            // Act
            _notifierUnderTest.Handle(martenEvent);

            //Act
            var notifications = Session.Query<INotification>().ToList();
            notifications.Cast<EventNotification>()
                .All(x => x.EventId == eventId && x.EventType == typeof(WorkpackageOneReviewRequestedMentoringEvent))
                .Should().BeTrue();

            notifications.Count.Should().Be(administrators.Count);

            var expectedUserIds = administrators.Select(x => x.UserId);
            notifications.Select(n => n.NotificationTo).Should().BeEquivalentTo(expectedUserIds);
        }
    }
}
