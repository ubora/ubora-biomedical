using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FluentAssertions;
using Marten;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;
using Xunit;

namespace Ubora.Domain.Tests.NotifierTests
{
    /// <summary>
    /// Test for <see cref="ProjectCreatedEvent.Notifier"/>
    /// </summary>
    public class ProjectCreatedEventNotifierTests : IntegrationFixture
    {
        private readonly TestFindUboraMentorProfilesQueryHandler _findUboraMentorProfilesQueryHandler = new TestFindUboraMentorProfilesQueryHandler();

        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            builder.RegisterInstance(_findUboraMentorProfilesQueryHandler)
                .As<IQueryHandler<FindUboraMentorProfilesQuery, IReadOnlyCollection<UserProfile>>>()
                .SingleInstance();
        }

        [Fact]
        public void Notifies_All_Ubora_Mentors()
        {
            var projectCreatorUserId = Guid.NewGuid();
            this.Create_User(projectCreatorUserId);

            var uboraMentorUserProfiles = new[]
            {
                new UserProfile(Guid.NewGuid()),
                new UserProfile(Guid.NewGuid()),
                new UserProfile(Guid.NewGuid()),
            };
            _findUboraMentorProfilesQueryHandler.UserMentorProfilesToReturn = uboraMentorUserProfiles;
            
            Session.DeleteWhere<INotification>(_ => true);

            // Act
            Processor.Execute(new CreateProjectCommand
            {
                Actor = new UserInfo(projectCreatorUserId, "")
            });

            // Assert
            var projectCreatedEvent = Session.Events.FindLastEvent();
            
            var notifications = Session.Query<INotification>().ToList();
            notifications.Cast<EventNotification>()
                .All(x => x.EventId == projectCreatedEvent.Id && x.EventType == typeof(ProjectCreatedEvent))
                .Should().BeTrue();

            notifications.Count.Should().Be(uboraMentorUserProfiles.Count());

            var expectedUserIds = uboraMentorUserProfiles.Select(x => x.UserId);
            notifications.Select(n => n.NotificationTo).Should().BeEquivalentTo(expectedUserIds);
        }
    }
}