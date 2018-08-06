using System;
using System.Linq;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Users;
using Xunit;
using UserProfile = Ubora.Domain.Users.UserProfile;

namespace Ubora.Domain.Tests.Projects.Members.Commands
{
    public class InviteProjectMentorCommandHandlerTests
    {
        private readonly InviteProjectMentorCommand.Handler _handlerUnderTest;
        private readonly Mock<IDocumentSession> _documentSessionStrictMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;

        public InviteProjectMentorCommandHandlerTests()
        {
            _documentSessionStrictMock = new Mock<IDocumentSession>(MockBehavior.Strict);
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _handlerUnderTest = new InviteProjectMentorCommand.Handler(_documentSessionStrictMock.Object, _queryProcessorMock.Object);
        }

        public Guid ProjectId = Guid.NewGuid();
        public Guid UserId = Guid.NewGuid();

        [Fact]
        public void Handle_Fails_When_User_Already_Mentor_Of_Project()
        {
            var project = Mock.Of<Project>(x => x.DoesSatisfy(new HasMember<ProjectMentor>(UserId)));

            _documentSessionStrictMock.Setup(x => x.Load<Project>(ProjectId))
                .Returns(project);

            _documentSessionStrictMock.Setup(x => x.Load<UserProfile>(UserId))
                .Returns(new UserProfile(UserId));

            var command = new InviteProjectMentorCommand
            {
                ProjectId = ProjectId,
                UserId = UserId
            };

            // Act
            var result = _handlerUnderTest.Handle(command);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Handle_Fails_When_User_Already_Invited()
        {
            var project = new Project().Set(x => x.Id, ProjectId);

            _documentSessionStrictMock.Setup(x => x.Load<Project>(ProjectId))
                .Returns(project);

            var userProfile = new UserProfile(UserId);

            _documentSessionStrictMock.Setup(x => x.Load<UserProfile>(UserId))
                .Returns(userProfile);

            var expectedSpec = new IsFromProjectSpec<ProjectMentorInvitation>{ProjectId = ProjectId} &&
                new HasPendingNotifications<ProjectMentorInvitation>(UserId);
            var invitationsBySpec = new PagedListStub<ProjectMentorInvitation>
            {
                new ProjectMentorInvitation(inviteeUserId: Guid.NewGuid(), projectId: Guid.NewGuid(), invitedBy: Guid.NewGuid()),
            };

            _queryProcessorMock.Setup(x => x.Find(expectedSpec))
                .Returns(invitationsBySpec);

            var command = new InviteProjectMentorCommand
            {
                ProjectId = ProjectId,
                UserId = UserId
            };

            // Act
            var result = _handlerUnderTest.Handle(command);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Handle_Succeeds_When_Valid()
        {
            var project = new Project().Set(x => x.Id, ProjectId);

            _documentSessionStrictMock.Setup(x => x.Load<Project>(ProjectId))
                .Returns(project);

            var userProfile = new UserProfile(UserId);

            _documentSessionStrictMock.Setup(x => x.Load<UserProfile>(UserId))
                .Returns(userProfile);

            var expectedSpec = new IsFromProjectSpec<ProjectMentorInvitation> { ProjectId = ProjectId } 
                && new HasPendingNotifications<ProjectMentorInvitation>(UserId);

            _queryProcessorMock.Setup(x => x.Find(expectedSpec))
                .Returns(new PagedListStub<ProjectMentorInvitation>());

            ProjectMentorInvitation storedInvitation = null;
            _documentSessionStrictMock.Setup(x => x.Store(It.IsAny<ProjectMentorInvitation[]>()))
                .Callback<ProjectMentorInvitation[]>(invitations => storedInvitation = invitations.Single());

            _documentSessionStrictMock.Setup(x => x.SaveChanges());

            var invitedBy = new UserInfo(Guid.NewGuid(), "");
            var command = new InviteProjectMentorCommand
            {
                ProjectId = ProjectId,
                UserId = UserId,
                Actor = invitedBy
            };

            // Act
            var result = _handlerUnderTest.Handle(command);

            // Assert
            result.IsSuccess.Should().BeTrue();
            storedInvitation.ProjectId.Should().Be(ProjectId);
            storedInvitation.InviteeUserId.Should().Be(UserId);
            storedInvitation.InvitedBy.Should().Be(invitedBy.UserId);
        }
    }
}
