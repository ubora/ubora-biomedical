﻿using System;
using Xunit;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects.Members;

namespace Ubora.Domain.Tests.Notifications
{
    public class HasArchivedNotificationsTests : IntegrationFixture
    {
        [Fact]
        public void Returns_Notifications_That_Have_Been_Viewed()
        {
            var expectedUserId = Guid.NewGuid();

            var invitationToProject = new InvitationToProject(expectedUserId, Guid.NewGuid());
            var expectedInvitation = new InvitationToProject(expectedUserId, Guid.NewGuid());
            expectedInvitation.Accept();

            Session.Store(invitationToProject);
            Session.Store(expectedInvitation);
            Session.SaveChanges();

            var sut = new HasArchivedNotifications<InvitationToProject>(expectedUserId);
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
