using System;
using FluentAssertions;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Queries;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Queries
{
    public class GetProjectsUnderReviewQueryTests : IntegrationFixture
    {
        [Fact]
        public void Returns_Projects_That_Have_WorkpackageOne_Under_Review()
        {
            var projectUnderReviewId = Guid.NewGuid();
            var otherProjectUnderReviewId = Guid.NewGuid();
            var projectWithNotSubmittedReviewId = Guid.NewGuid();
            var projectWithAcceptedReviewId = Guid.NewGuid();

            this.Create_Project(projectUnderReviewId);
            this.Submit_Workpackage_One_For_Review(projectUnderReviewId);

            this.Create_Project(otherProjectUnderReviewId);
            this.Submit_Workpackage_One_For_Review(otherProjectUnderReviewId);

            this.Create_Project(projectWithNotSubmittedReviewId);

            this.Create_Project(projectWithAcceptedReviewId);
            this.Submit_Workpackage_One_For_Review(projectWithAcceptedReviewId);
            this.Accept_Workpackage_One_Review(projectWithAcceptedReviewId);

            // Act
            var result = Processor.ExecuteQuery(new GetProjectsUnderReviewQuery());

            // Assert
            var expectedProject1 = Processor.FindById<Project>(projectUnderReviewId);
            var expectedProject2 = Processor.FindById<Project>(otherProjectUnderReviewId);
            result.ShouldBeEquivalentTo(new [] { expectedProject1, expectedProject2 });
        }
    }
}
