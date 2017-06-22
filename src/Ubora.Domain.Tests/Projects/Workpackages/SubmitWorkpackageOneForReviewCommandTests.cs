using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class SubmitWorkpackageOneForReviewCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();

        [Fact]
        public void Workpackage_Can_Be_Submitted_For_Review()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .Then(_ => this.Assert_Workpackage_One_Is_In_Single_Review())
                .BDDfy();
        }

        [Fact]
        public void Workpackage_Can_Be_Submitted_For_Review_Again_After_Rejection()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                    .And(_ => this.Reject_Workpackage_One_Review(_projectId))
                .When(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .Then(_ => this.Assert_Workpackage_One_Is_In_Review())
                .BDDfy();
        }

        [Fact]
        public void Workpackage_Can_Not_Be_Submitted_For_Review_Again_After_Acceptance()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                    .And(_ => this.Accept_Workpackage_One_Review(_projectId))
                .When(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .Then(_ => this.Assert_Workpackage_One_Has_Single_Accepted_Review())
                .BDDfy();
        }

        [Fact]
        public void Workpackage_Can_Not_Be_In_Multiple_Reviews_At_Once()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .Then(_ => this.Assert_Workpackage_One_Has_Single_Accepted_Review())
                .BDDfy();
        }

        protected void Assert_Workpackage_One_Is_In_Single_Review()
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(_projectId);

            var review = workpackageOne.Reviews.Single();
            review.Status.Should().Be(WorkpackageReviewStatus.InProcess);
            review.SubmittedAt.Should().BeCloseTo(DateTimeOffset.Now, 500);
            review.ConcludedAt.Should().BeNull();
        }

        protected void Assert_Workpackage_One_Is_In_Review()
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(_projectId);

            var isInReview = workpackageOne.DoesSatisfy(new HasReviewInStatus<WorkpackageOne>(WorkpackageReviewStatus.InProcess));
            isInReview.Should().BeTrue();
        }

        protected void Assert_Workpackage_One_Has_Single_Accepted_Review()
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(_projectId);

            workpackageOne.Reviews.Count.Should().Be(1);
        }
    }
}
