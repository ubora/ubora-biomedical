using Autofac;
using FluentAssertions;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Users;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public class InviteMemberToProjectTests : IntegrationFixture
    {
        public InviteMemberToProjectTests()
        {
            StoreOptions(new UboraStoreOptions().Configuration());
        }

        [Fact]
        public void Invite_Member_Adds_Member_To_Project()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var projectId = Guid.NewGuid();
            processor.Execute(new CreateProjectCommand
            {
                UserInfo = new UserInfo(Guid.NewGuid(), ""),
                Id = projectId
            });

            var userId = Guid.NewGuid();

            processor.Execute(new CreateUserProfileCommand
            {
                UserId = userId
            });

            var expectedUserInfo = new UserInfo(Guid.NewGuid(), "");

            var command = new InviteMemberToProjectCommand
            {
                ProjectId = projectId,
                UserId = userId,
                UserInfo = expectedUserInfo
            };

            // Act
            processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(projectId);

            project.Members.Any(m => m.UserId == userId).Should().BeTrue();
        }
    }
}
