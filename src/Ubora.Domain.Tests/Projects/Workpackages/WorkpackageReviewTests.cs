using System;
using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class WorkpackageReviewTests
    {
        [Fact]
        public void ToWorkpackageReopened_Throws_When_Previous_Status_Is_NOT_Accepted()
        {
            var workpackageReview = WorkpackageReview.Create(id: Guid.NewGuid(), submittedAt: DateTime.UtcNow);

            // Act
            Action act = () => workpackageReview.ToWorkpackageReopened();

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ToWorkpackageReopened_Only_Changes_Status()
        {
            var initialReview = WorkpackageReview.Create(id: Guid.NewGuid(), submittedAt: DateTime.UtcNow)
                .ToAccepted(concludingComment: "testConcludingComment", concludedAt: DateTimeOffset.UtcNow.AddMinutes(1));

            // Act
            var result = initialReview.ToWorkpackageReopened();

            // Assert
            result.ShouldBeEquivalentTo(initialReview, o => o.Excluding(r => r.Status));
            result.Status.Should().Be(WorkpackageReviewStatus.AcceptedButWorkpackageReopened);
        }
    }
}
