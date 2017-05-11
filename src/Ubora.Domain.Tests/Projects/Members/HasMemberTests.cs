using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Members
{
    public class HasMemberTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_Projects_Which_Have_User_As_Member()
        {
			// Insert dummy project
            Processor.Execute(new CreateProjectCommand
            {
                Id = Guid.NewGuid(),
                UserInfo = new UserInfo(Guid.NewGuid(), "")
            });

            var userId = Guid.NewGuid();
            var expectedProjectId = Guid.NewGuid();

            Processor.Execute(new CreateProjectCommand
            {
                Id = expectedProjectId,
                UserInfo = new UserInfo(userId, "") // Will be first member.
            });

            var sut = new HasMember(userId);

            // Act
            var result = sut.SatisfyEntitiesFrom(Session.Query<Project>());

            // Assert
            result.Single().Id.Should().Be(expectedProjectId);
        }
    }
}
