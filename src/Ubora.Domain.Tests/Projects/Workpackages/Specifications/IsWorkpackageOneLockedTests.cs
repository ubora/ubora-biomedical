using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Specifications
{
    public class IsWorkpackageOneLockedTests
    {
        private readonly IsWorkpackageOneLocked _specification = new IsWorkpackageOneLocked();

        [Fact]
        public void Returns_True_When_Workpackage_Has_Review_In_Process()
        {
            var workpackage = WorkpackageOneFactory.Create(reviewsInStatus: WorkpackageReviewStatus.InProcess);

            // Act
            var result = _specification.IsSatisfiedBy(workpackage);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_True_When_Workpackage_Has_Been_Accepted_By_Review()
        {
            var workpackage = WorkpackageOneFactory.Create(reviewsInStatus: WorkpackageReviewStatus.Accepted);

            // Act
            var result = _specification.IsSatisfiedBy(workpackage);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_New_Workpackage_Is_Not_In_Review_Or_Has_Not_Been_Accepted_By_Review()
        {
            var workpackage = WorkpackageOneFactory.Create(reviewsInStatus: WorkpackageReviewStatus.Rejected);

            // Act
            var result = _specification.IsSatisfiedBy(workpackage);

            // Assert
            result.Should().BeFalse();
        }
    }
}
