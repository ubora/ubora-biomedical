﻿using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Notifications
{
    public class NonViewedInvitationsTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_UserInvitations_That_Have_Not_Been_Viewed()
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

            Processor.Execute(new MarkInvitationsAsViewedCommand
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

            var sut = new NonViewedInvitations(userId);
            var invitations = Session.Query<InvitationToProject>();

            // Act
            var result = sut.SatisfyEntitiesFrom(invitations);

            // Assert
            var invitation = result.Single();

            invitation.HasBeenViewed.Should().BeFalse();
            invitation.InvitedMemberId.Should().Be(expectedUserId);
        }
    }
}
