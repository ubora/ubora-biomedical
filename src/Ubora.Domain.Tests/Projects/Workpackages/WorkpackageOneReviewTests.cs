using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.WorkpackageOnes;
using Ubora.Domain.Projects.WorkpackageTwos;
using Ubora.Domain.Tests.Projects.Members;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class WorkpackageOneReviewTests : IntegrationFixture
    {
        [Fact]
        public void Workpackage_Can_Be_Submitted_For_Review()
        {
            var projectId = Guid.NewGuid();

            this.Given(_ => this.Given_There_Is_Project(projectId))
                .When(_ => SubmitWorkpackageForReview(projectId))
                .Then(_ => AssertWorkpackageOneHasReviewInStatus(projectId, WorkpackageReviewStatus.InReview))
                .BDDfy();
        }

        private void SubmitWorkpackageForReview(Guid workpackageId)
        {
            Processor.Execute(new SubmitWorkpackageOneForReviewCommand
            {
                ProjectId = workpackageId,
                Actor = new DummyUserInfo()
            });
        }

        [Fact]
        public void Workpackage_Can_Be_Accepted_By_Review()
        {
            var projectId = Guid.NewGuid();

            this.Given(_ => this.Given_There_Is_Project(projectId))
                // TODO
                    .And(_ => SubmitWorkpackageForReview(projectId))
                .When(_ => AcceptWorkpackageOneByReview(projectId))
                .Then(_ => AssertWorkpackageOneHasReviewInStatus(projectId, WorkpackageReviewStatus.Accepted))
                    .And(_ => AssertWorkpackageTwoIsOpened(projectId))
                .BDDfy();
        }


        private void AcceptWorkpackageOneByReview(Guid workpackageId)
        {
            Processor.Execute(new AcceptWorkpackageOneByReviewCommand
            {
                ProjectId = workpackageId,
                Actor = new DummyUserInfo()
            });
        }

        private void AssertWorkpackageTwoIsOpened(Guid projectId)
        {
            var workpackageTwo = Processor.FindById<WorkpackageTwo>(projectId);

            workpackageTwo.Should().NotBeNull();
        }

        private void AssertWorkpackageOneHasReviewInStatus(Guid workpackageId, WorkpackageReviewStatus status)
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(workpackageId);

            var isAccepted = workpackageOne.DoesSatisfy(new HasReviewInStatus<WorkpackageOne>(status));
            isAccepted.Should().BeTrue();
        }

        [Fact]
        public void Workpackage_Can_Be_Rejected_By_Review()
        {
            var projectId = Guid.NewGuid();

            this.Given(_ => this.Given_There_Is_Project(projectId))
                // TODO
                    .And(_ => SubmitWorkpackageForReview(projectId))
                .When(_ => RejectWorkpackageOneByReview(projectId))
                .Then(_ => AssertWorkpackageOneHasReviewInStatus(projectId, WorkpackageReviewStatus.Rejected))
                .BDDfy();
        }

        private void RejectWorkpackageOneByReview(Guid workpackageId)
        {
            Processor.Execute(new RejectWorkpackageOneByReviewCommand
            {
                ProjectId = workpackageId,
                Actor = new DummyUserInfo()
            });
        }

        [Fact]
        public void Workpackage_Can_Not_Be_In_Multiple_Reviews_At_Once()
        {
            var projectId = Guid.NewGuid();

            this.Given(_ => this.Given_There_Is_Project(projectId))
                .When(_ => SubmitWorkpackageForReview(projectId))
                    .And(_ => SubmitWorkpackageForReview(projectId))
                .Then(_ => AssertExistenceOfSingleReviewForWorkpackage(projectId))
                .BDDfy();
        }

        private void AssertExistenceOfSingleReviewForWorkpackage(Guid workpackageId)
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(workpackageId);

            workpackageOne.Reviews.Count
                .Should().Be(1);
        }

        [Fact]
        public void Workpackage_Can_Be_Submitted_For_Review_Again_After_Rejection()
        {
            var projectId = Guid.NewGuid();

            this.Given(_ => this.Given_There_Is_Project(projectId))
                // TODO
                    .And(_ => SubmitWorkpackageForReview(projectId))
                    .And(_ => RejectWorkpackageOneByReview(projectId))
                .When(_ => SubmitWorkpackageForReview(projectId))
                .Then(_ => AssertWorkpackageOneHasReviewInStatus(projectId, WorkpackageReviewStatus.InReview))
                .BDDfy();
        }

        [Fact]
        public void Workpackage_Can_Not_Be_Submitted_For_Review_Again_After_Acceptance()
        {
            var projectId = Guid.NewGuid();

            this.Given(_ => this.Given_There_Is_Project(projectId))
                // TODO
                    .And(_ => SubmitWorkpackageForReview(projectId))
                    .And(_ => AcceptWorkpackageOneByReview(projectId))
                .When(_ => SubmitWorkpackageForReview(projectId))
                .Then(_ => AssertExistenceOfSingleReviewForWorkpackage(projectId))
                    .And(_ => AssertWorkpackageOneHasReviewInStatus(projectId, WorkpackageReviewStatus.Accepted))
                .BDDfy();
        }

        [Fact]
        public void Workpackage_Can_Not_Be_Edited_When_In_Review()
        {
            // todo
        }
    }
}
