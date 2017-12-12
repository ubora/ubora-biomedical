using System;
using FluentAssertions;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Queries;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Queries
{
    public class FindUserProjectsQueryTests : IntegrationFixture
    {
        [Fact]
        public void Returns_User_Projects()
        {
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var userProject1Id = Guid.NewGuid();
            var userProject2Id = Guid.NewGuid();
            var otherProjectId = Guid.NewGuid();

            this.Create_Project(userProject1Id, userId);
            this.Create_Project(userProject2Id, userId);
            this.Create_Project(otherProjectId, otherUserId);

            var findUserProjectsQuery = new FindUserProjectsQuery { UserId = userId };

            // Act
            var result = Processor.ExecuteQuery(findUserProjectsQuery);

            // Assert
            var expectedProject1 = Processor.FindById<Project>(userProject1Id);
            var expectedProject2 = Processor.FindById<Project>(userProject2Id);
            result.ShouldBeEquivalentTo(new[] { expectedProject1, expectedProject2 });
        }
    }
}
