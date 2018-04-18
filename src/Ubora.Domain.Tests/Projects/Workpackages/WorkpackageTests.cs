using System;
using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class WorkpackageTests
    {
        [Fact]
        public void GetLatestReview_Returns_Latest_Submitted_Review()
        {
            var wp = new TestWorkpackage();
            var expectedReviewId = Guid.NewGuid();

            wp.AddReview(WorkpackageReview.Create(id: Guid.NewGuid(), submittedAt: DateTime.Now.AddDays(-3)));
            wp.AddReview(WorkpackageReview.Create(id: expectedReviewId, submittedAt: DateTime.Now));
            wp.AddReview(WorkpackageReview.Create(id: Guid.NewGuid(), submittedAt: DateTime.Now.AddDays(-2)));
            wp.AddReview(WorkpackageReview.Create(id: Guid.NewGuid(), submittedAt: DateTime.Now.AddDays(-1)));

            // Act
            var result = wp.GetLatestReviewOrNull();

            // Assert
            result.Id.Should().Be(expectedReviewId);
        }

        public class TestWorkpackage : Workpackage<TestWorkpackage>
        {
            public void AddReview(WorkpackageReview review)
            {
                _reviews.Add(review);
            }
        }
    }
}
