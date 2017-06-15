using System;
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
                .Then(_ => this.Assert_Workpackage_One_Has_Review_In_Status(_projectId, WorkpackageReviewStatus.InReview))
                .BDDfy();
        }

        [Fact]
        public void Workpackage_Can_Be_Submitted_For_Review_Again_After_Rejection()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                    .And(_ => this.Reject_Workpackage_One_Review(_projectId))
                .When(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .Then(_ => this.Assert_Workpackage_One_Has_Review_In_Status(_projectId, WorkpackageReviewStatus.InReview))
                .BDDfy();
        }

        [Fact]
        public void Workpackage_Can_Not_Be_Submitted_For_Review_Again_After_Acceptance()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                    .And(_ => this.Accept_Workpackage_One_Review(_projectId))
                .When(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .Then(_ => this.Assert_Existence_Of_Single_Review_For_Workpackage(_projectId))
                    .And(_ => this.Assert_Workpackage_One_Has_Review_In_Status(_projectId, WorkpackageReviewStatus.Accepted))
                .BDDfy();
        }

        [Fact]
        public void Workpackage_Can_Not_Be_In_Multiple_Reviews_At_Once()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .Then(_ => this.Assert_Existence_Of_Single_Review_For_Workpackage(_projectId))
                .BDDfy();
        }

        protected void Assert_Workpackage_One_Has_Review_In_Status(Guid workpackageId, WorkpackageReviewStatus status)
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(workpackageId);

            var isAccepted = workpackageOne.DoesSatisfy(new HasReviewInStatus<WorkpackageOne>(status));
            isAccepted.Should().BeTrue();
        }

        protected void Assert_Existence_Of_Single_Review_For_Workpackage(Guid workpackageId)
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(workpackageId);

            workpackageOne.Reviews.Count.Should().Be(1);
        }
    }
}
