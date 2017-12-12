using System;
using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class WorkpackageOneTests
    {
        [Fact]
        public void Apply_For_WorkpackageOneReopenedAfterAcceptanceByReviewEvent_Throws_When_Given_Review_Id_Is_Not_Latest()
        {
            var wp1 = new TestWorkpackageOne();

            var notLatestReviewId = Guid.NewGuid();
            var notLatestReview = WorkpackageReview.Create(id: notLatestReviewId, submittedAt: DateTime.Now)
                .Set(r => r.Status, WorkpackageReviewStatus.Accepted);

            wp1.AddReview(WorkpackageReview.Create(id: Guid.NewGuid(), submittedAt: DateTime.Now.AddDays(2)));
            wp1.AddReview(notLatestReview);
            wp1.AddReview(WorkpackageReview.Create(id: Guid.NewGuid(), submittedAt: DateTime.Now.AddDays(1)));

            var @event = new WorkpackageOneReopenedAfterAcceptanceByReviewEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: Guid.Empty,
                acceptedReviewId: notLatestReviewId);
            
            // Act
            Action act = () => wp1.Apply(@event);

            // Assert
            act.ShouldThrow<InvalidOperationException>().And.Message.Should().Contain("latest");
        }
    }

    public class TestWorkpackageOne : WorkpackageOne
    {
        public void AddReview(WorkpackageReview review)
        {
            _reviews.Add(review);
        }
    }
}
