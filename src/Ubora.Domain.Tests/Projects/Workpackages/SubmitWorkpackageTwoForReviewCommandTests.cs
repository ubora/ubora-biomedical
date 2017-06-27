using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.Workpackages;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class SubmitWorkpackageTwoForReviewCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();

        [Fact]
        public void Workpackage_Two_Can_Be_Submitted_For_Review()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                    .And(_ => this.Accept_Workpackage_One_Review(_projectId))
                .When(_ => this.Submit_Workpackage_Two_For_Review(_projectId))
                .Then(_ => this.Assert_Workpackage_Two_Is_In_Single_Review())
                .BDDfy();
        }

        protected void Assert_Workpackage_Two_Is_In_Single_Review()
        {
            var workpackageOne = Processor.FindById<WorkpackageTwo>(_projectId);

            var review = workpackageOne.Reviews.Single();
            review.Status.Should().Be(WorkpackageReviewStatus.InProcess);
            review.SubmittedAt.Should().BeCloseTo(DateTimeOffset.Now, 500);
            review.ConcludedAt.Should().BeNull();
        }
    }
}
