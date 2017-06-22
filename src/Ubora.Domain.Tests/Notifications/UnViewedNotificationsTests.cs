using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Notifications.Join;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Notifications
{
    public class UnViewedNotificationsTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_UserNotifications_That_Have_Not_Been_Viewed()
        {
            var userId = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var expectedUserId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            Processor.Execute(new CreateUserProfileCommand
            {
                UserId = userId,
                Email = "foo@goo.com",
                Actor = new UserInfo(userId, "")
            });
            Processor.Execute(new CreateUserProfileCommand
            {
                UserId = userId1,
                Email = "jane@doe.com",
                Actor = new UserInfo(userId, "")
            });
            Processor.Execute(new CreateUserProfileCommand
            {
                UserId = expectedUserId,
                Email = "jake@doe.com",
                Actor = new UserInfo(userId, "")
            });

            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Title = "title",
                Actor = new UserInfo(userId, "")
            });

            Processor.Execute(new JoinProjectCommand
            {
                ProjectId = projectId,
                AskingToJoin = userId1,
                Actor = new UserInfo(userId, "")
            });

            Processor.Execute(new MarkNotificationsAsViewedCommand
            {
                UserId = userId,
                Actor = new UserInfo(userId, "")
            });

            Processor.Execute(new JoinProjectCommand
            {
                ProjectId = projectId,
                AskingToJoin = expectedUserId,
                Actor = new UserInfo(userId, "")
            });

            var sut = new UnViewedNotifications<RequestToJoinProject>(userId);
            var invitations = Session.Query<RequestToJoinProject>();

            // Act
            var result = sut.SatisfyEntitiesFrom(invitations);

            // Assert
            var invitation = result.Single();

            invitation.HasBeenViewed.Should().BeFalse();
            invitation.AskingToJoinMemberId.Should().Be(expectedUserId);
        }
    }
}
