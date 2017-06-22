using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Specifications
{
    public class CanSubmitWorkpackageReviewTests
    {
        [Fact]
        public void Returns_False_When_Workpackage_Has_Review_In_Process()
        {
            var workpackage = WorkpackageOneFactory.Create(reviewsInStatus: WorkpackageReviewStatus.InProcess);

            var specification = new CanSubmitWorkpackageReview<WorkpackageOne>();

            // Act
            var result = specification.IsSatisfiedBy(workpackage);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_Workpackage_Has_Been_Accepted_By_Review()
        {
            var workpackage = WorkpackageOneFactory.Create(reviewsInStatus: WorkpackageReviewStatus.Accepted);

            var specification = new CanSubmitWorkpackageReview<WorkpackageOne>();

            // Act
            var result = specification.IsSatisfiedBy(workpackage);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Return_True_Otherwise()
        {
            var workpackage1 = WorkpackageOneFactory.Create();
            var workpackage2 = WorkpackageOneFactory.Create(reviewsInStatus: WorkpackageReviewStatus.Rejected);

            var specification = new CanSubmitWorkpackageReview<WorkpackageOne>();

            // Act
            var result1 = specification.IsSatisfiedBy(workpackage1);
            var result2 = specification.IsSatisfiedBy(workpackage2);

            // Assert
            result1.Should().BeTrue();
            result2.Should().BeTrue();
        }
    }
}
