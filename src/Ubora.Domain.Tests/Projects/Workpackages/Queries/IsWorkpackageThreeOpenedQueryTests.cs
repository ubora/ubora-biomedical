using System;
using FluentAssertions;
using Ubora.Domain.Projects.Workpackages.Queries;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Queries
{
    public class IsWorkpackageThreeOpenedQueryTests : IntegrationFixture
    {
        [Fact]
        public void Returns_Whether_Project_Has_Workpackage_Three_Opened()
        {
            var projectIdWithWp3 = Guid.NewGuid();
            var otherProjectIdWithWp3 = Guid.NewGuid();
            var projectIdWithoutWp3 = Guid.NewGuid();

            Create_Project_With_WP3(projectIdWithWp3);
            Create_Project_With_WP3(otherProjectIdWithWp3);
            Create_Project_Without_WP3(projectIdWithoutWp3);

            // Act
            var resultForProjectWithWp3 = Processor.ExecuteQuery(new IsWorkpackageThreeOpenedQuery(projectIdWithWp3));
            var resultForProjectWithoutWp3 = Processor.ExecuteQuery(new IsWorkpackageThreeOpenedQuery(projectIdWithoutWp3));

            // Assert
            resultForProjectWithWp3.Should().BeTrue();
            resultForProjectWithoutWp3.Should().BeFalse();
        }

        private void Create_Project_Without_WP3(Guid projectIdWithoutWp3)
        {
            this.Create_Project(projectIdWithoutWp3);
        }

        private void Create_Project_With_WP3(Guid projectId)
        {
            this.Create_Project(projectId);
            this.Submit_Workpackage_One_For_Review(projectId);
            this.Accept_Workpackage_One_Review(projectId);
            this.Open_Workpackage_Three(projectId);
        }
    }
}
