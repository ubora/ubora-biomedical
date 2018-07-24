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
        private readonly TestFindUboraManagementGroupHandler _findUboraManagementGroupHandler = new TestFindUboraManagementGroupHandler();

        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            builder.RegisterInstance(_findUboraManagementGroupHandler)
                .As<IQueryHandler<FindUboraManagementGroupQuery, IReadOnlyCollection<UserProfile>>>()
                .SingleInstance();

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
            var managementGroupUsers = new List<UserProfile> { new UserProfile(Guid.NewGuid()), new UserProfile(Guid.NewGuid()) };
            _findUboraManagementGroupHandler.UboraManagementGroupProfilesToReturn = managementGroupUsers;

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

            var managementGroupUserAndAdministratorsIds = administrators.Select(x => x.UserId).Concat(managementGroupUsers.Select(x => x.UserId));

            notifications.Count.Should().Be(managementGroupUserAndAdministratorsIds.Count());
            notifications.Select(n => n.NotificationTo).Should().BeEquivalentTo(managementGroupUserAndAdministratorsIds);
        }
    }
}
