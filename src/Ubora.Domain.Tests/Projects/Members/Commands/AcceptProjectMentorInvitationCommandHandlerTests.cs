using System;
using System.Linq;
using FluentAssertions;
using Marten;
using Marten.Events;
using Moq;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Domain.Projects._Events;
using Xunit;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Tests.Projects.Members.Commands
{
    public class AcceptProjectMentorInvitationCommandHandlerTests
    {
        private readonly AcceptProjectMentorInvitationCommand.Handler _handlerUnderTest;
        private readonly Mock<IDocumentSession> _documentSessionStrictMock;

        public AcceptProjectMentorInvitationCommandHandlerTests()
        {
            _documentSessionStrictMock = new Mock<IDocumentSession>(MockBehavior.Strict);
            _handlerUnderTest = new AcceptProjectMentorInvitationCommand.Handler(_documentSessionStrictMock.Object);
        }

        [Fact]
        public void Handle_Accepts_Invitation_And_Stores_Event_And_Notification()
        {
            var projectId = Guid.NewGuid();
            var inviteeUserId = Guid.NewGuid();
            var invitation = new ProjectMentorInvitation(
                inviteeUserId: inviteeUserId,
                projectId: projectId,
                invitedBy: Guid.NewGuid());

            _documentSessionStrictMock.Setup(x => x.Load<ProjectMentorInvitation>(invitation.Id))
                .Returns(invitation);

            _documentSessionStrictMock.Setup(x => x.Load<WorkpackageOne>(projectId))
                .Returns(new WorkpackageOne());

            _documentSessionStrictMock.Setup(x => x.Store(It.IsAny<WorkpackageOne[]>()));

            MentorJoinedProjectEvent storedEvent = null;
            _documentSessionStrictMock.Setup(x => x.Events.Append(projectId, It.IsAny<object[]>()))
                .Callback<Guid, object[]>((streamId, events) => storedEvent = (MentorJoinedProjectEvent)events.Single())
                .Returns((EventStream)null);

            ProjectMentorInvitation storedInvitation = null;
            _documentSessionStrictMock.Setup(x => x.Store(It.IsAny<ProjectMentorInvitation[]>()))
                .Callback<ProjectMentorInvitation[]>(y => storedInvitation = y.Single());

            MentorJoinedProjectEvent.NotificationToInviter storedNotification = null;
            _documentSessionStrictMock.Setup(x => x.Store(It.IsAny<MentorJoinedProjectEvent.NotificationToInviter[]>()))
                .Callback<MentorJoinedProjectEvent.NotificationToInviter[]>(y => storedNotification = y.Single());

            _documentSessionStrictMock.Setup(x => x.SaveChanges());

            var command = new AcceptProjectMentorInvitationCommand
            {
                InvitationId = invitation.Id,
                Actor = new DummyUserInfo()
            };

            // Act
            var result = _handlerUnderTest.Handle(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            storedEvent.ProjectId.Should().Be(projectId);
            storedEvent.UserId.Should().Be(inviteeUserId);

            storedInvitation.Id.Should().Be(invitation.Id);
            storedInvitation.IsAccepted.Should().BeTrue();

            storedNotification.ProjectId.Should().Be(projectId);
            storedNotification.JoinerId.Should().Be(inviteeUserId);
        }
    }
}