using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class WorkpackageTwoTests
    {
        [Fact]
        public void Apply_For_WorkpackageOneReviewAcceptedEvent_Throws_When_Applied_Multiple_Times_In_Row()
        {
            var @event = new WorkpackageOneReviewAcceptedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: Guid.NewGuid(),
                concludingComment: "",
                acceptedAt: DateTimeOffset.UtcNow);

            var wp2 = new WorkpackageTwo();
            wp2.Apply(@event);

            // Act
            Action act = () => wp2.Apply(@event);

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Apply_For_WorkpackageOneReviewAcceptedEvent_Does_Not_Change_Steps_When_Wp1_Was_Reopened_And_Accepted_Again()
        {
            var acceptedEvent = new WorkpackageOneReviewAcceptedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: Guid.NewGuid(),
                concludingComment: "",
                acceptedAt: DateTimeOffset.UtcNow);

            var reopenedEvent = new WorkpackageOneReopenedAfterAcceptanceByReviewEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: Guid.NewGuid(),
                acceptedReviewId: Guid.NewGuid());

            var editRandomStepEvent = new WorkpackageTwoStepEdited(
                initiatedBy: new DummyUserInfo(),
                projectId: Guid.NewGuid(),
                stepId: "PhysicalPrinciples",
                title: "testValue1",
                newValue: "testValue2");

            var wp2 = new WorkpackageTwo();
            wp2.Apply(acceptedEvent);
            wp2.Apply(editRandomStepEvent);

            var stepsBeforeAcceptingAgain = wp2.Steps.ToList();

            wp2.Apply(reopenedEvent);

            // Act
            wp2.Apply(acceptedEvent);

            // Assert
            wp2.Steps.ShouldBeEquivalentTo(stepsBeforeAcceptingAgain);
        }
    }
}
