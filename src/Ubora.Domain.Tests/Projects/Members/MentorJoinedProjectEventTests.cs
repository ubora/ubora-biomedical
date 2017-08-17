using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public class MentorJoinedProjectEventTests
    {
        [Fact]
        public void Apply_Throws_When_User_Already_Member()
        {
            var userId = Guid.NewGuid();

            var projectMock = new Mock<Project>();
            projectMock.Setup(x => x.DoesSatisfy(new HasMember<ProjectMentor>(userId)))
                .Returns(true);

            var @event = new MentorJoinedProjectEvent(
                projectId: Guid.NewGuid(),
                userId: userId,
                initiatedBy: new DummyUserInfo());

            // Act
            Action act = () => projectMock.Object.Apply(@event);

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Apply_Adds_New_Project_Mentor()
        {
            var project = new Project();
            var userId = Guid.NewGuid();

            var @event = new MentorJoinedProjectEvent(
                projectId: Guid.NewGuid(),
                userId: userId,
                initiatedBy: new DummyUserInfo());

            // Act
            project.Apply(@event);

            // Assert
            var joinedMember = project.Members.Single();

            joinedMember.UserId.Should().Be(userId);
        }
    }
}
