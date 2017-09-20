using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects._Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class HasMemberTests : IntegrationFixture
    {
        [Fact]
        public void Specification_Returns_Projects_Which_Have_User_As_Member()
        {
			// Insert dummy project
            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = Guid.NewGuid(),
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var userId = Guid.NewGuid();
            var expectedProjectId = Guid.NewGuid();

            Processor.Execute(new CreateProjectCommand
            {
                NewProjectId = expectedProjectId,
                Actor = new UserInfo(userId, "") // Will be first member.
            });

            var sut = new HasMember(userId);

            RefreshSession();

            var projectsQueryable = Session.Query<Project>();

            // Act
            var result = sut.SatisfyEntitiesFrom(projectsQueryable);

            // Assert
            var project = result.Single();

            project.Id.Should().Be(expectedProjectId);
        }
    }
}
