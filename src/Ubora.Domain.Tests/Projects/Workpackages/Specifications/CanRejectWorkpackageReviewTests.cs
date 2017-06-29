using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Specifications
{
    public class CanRejectWorkpackageReviewTests
    {
        [Fact]
        public void Returns_True_When_Workpackage_Has_Review_In_Process()
        {
            var workpackage = WorkpackageOneFactory.Create(reviewsInStatus: WorkpackageReviewStatus.InProcess);

            var specification = new CanRejectWorkpackageReview<WorkpackageOne>();

            // Act
            var result = specification.IsSatisfiedBy(workpackage);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_Workpackage_Does_Not_Have_Review_In_Process()
        {
            var workpackage = WorkpackageOneFactory.Create(reviewsInStatus: new[] { WorkpackageReviewStatus.Accepted, WorkpackageReviewStatus.Rejected });

            var specification = new CanRejectWorkpackageReview<WorkpackageOne>();

            // Act
            var result = specification.IsSatisfiedBy(workpackage);

            // Assert
            result.Should().BeFalse();
        }
    }
}