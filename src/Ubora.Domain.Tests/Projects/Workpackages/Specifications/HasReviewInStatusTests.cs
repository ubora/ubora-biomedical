using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Specifications
{
    public class HasReviewInStatusTests
    {
        [Fact]
        public void Returns_True_When_When_Workpackage_Has_Review_In_Given_Status()
        {
            var givenStatus = (WorkpackageReviewStatus) 123;
            var workpackage = WorkpackageOneFactory.Create(reviewsInStatus: new[]
            {
                WorkpackageReviewStatus.Accepted,
                givenStatus,
                WorkpackageReviewStatus.Rejected
            });
            
            var specification = new HasReviewInStatus<WorkpackageOne>(givenStatus);

            // Act
            var result = specification.IsSatisfiedBy(workpackage);
             
            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_When_Workpackage_Does_Not_Have_Review_In_Given_Status()
        {
            var givenStatus = (WorkpackageReviewStatus)123;
            var workpackage = WorkpackageOneFactory.Create(reviewsInStatus: new[]
            {
                WorkpackageReviewStatus.InProcess,
                WorkpackageReviewStatus.Accepted,
                WorkpackageReviewStatus.Rejected
            });

            var specification = new HasReviewInStatus<WorkpackageOne>(givenStatus);

            // Act
            var result = specification.IsSatisfiedBy(workpackage);

            // Assert
            result.Should().BeFalse();
        }
    }
}
