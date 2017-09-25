using System;
using System.Linq;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members.Commands
{
    public class DeclineProjectMentorInvitationCommandHandlerTests
    {
        private readonly DeclineProjectMentorInvitationCommand.Handler _handlerUnderTest;
        private readonly Mock<IDocumentSession> _documentSessionStrictMock;

        public DeclineProjectMentorInvitationCommandHandlerTests()
        {
            _documentSessionStrictMock = new Mock<IDocumentSession>(MockBehavior.Strict);
            _handlerUnderTest = new DeclineProjectMentorInvitationCommand.Handler(_documentSessionStrictMock.Object);
        }

        [Fact]
        public void Handle_Declines_Invitation_And_Stores_Event_And_Notification()
        {
            var projectId = Guid.NewGuid();
            var invitation = new ProjectMentorInvitation(
                inviteeUserId: Guid.NewGuid(),
                projectId: projectId,
                invitedBy: Guid.NewGuid());
            var invitee = new UserProfile(invitation.InviteeUserId);

            _documentSessionStrictMock.Setup(x => x.Load<ProjectMentorInvitation>(invitation.Id))
                .Returns(invitation);

            ProjectMentorInvitation storedInvitation = null;
            _documentSessionStrictMock.Setup(x => x.Store(It.IsAny<ProjectMentorInvitation[]>()))
                .Callback<ProjectMentorInvitation[]>(y => storedInvitation = y.Single());

            DeclineProjectMentorInvitationCommand.NotificationToInviter storedNotification = null;
            _documentSessionStrictMock.Setup(x => x.Store(It.IsAny<DeclineProjectMentorInvitationCommand.NotificationToInviter[]>()))
                .Callback<DeclineProjectMentorInvitationCommand.NotificationToInviter[]>(y => storedNotification = y.Single());

            _documentSessionStrictMock.Setup(x => x.SaveChanges());

            var command = new DeclineProjectMentorInvitationCommand
            {
                InvitationId = invitation.Id,
                Actor = new DummyUserInfo()
            };

            // Act
            var result = _handlerUnderTest.Handle(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            storedInvitation.Id.Should().Be(invitation.Id);
            storedInvitation.IsAccepted.Should().BeFalse();

            storedNotification.ProjectId.Should().Be(projectId);
            storedNotification.DeclinerUserId.Should().Be(invitee.UserId);
        }
    }
}
