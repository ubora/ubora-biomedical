using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.Workpackages;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class RejectWorkpackageTwoReviewCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();

        [Fact]
        public void Workpackage_Two_Review_Can_Be_Rejected()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                    .And(_ => this.Accept_Workpackage_One_Review(_projectId))
                    .And(_ => this.Submit_Workpackage_Two_For_Review(_projectId))
                .When(_ => this.Reject_Workpackage_Two_Review(_projectId))
                .Then(_ => this.Assert_Workpackage_Two_Has_Rejected_Review())
                .BDDfy();
        }

        protected void Assert_Workpackage_Two_Has_Rejected_Review()
        {
            var workpackageOne = Processor.FindById<WorkpackageTwo>(_projectId);

            var review = workpackageOne.Reviews.Single();
            review.Status.Should().Be(WorkpackageReviewStatus.Rejected);
            review.ConcludedAt.Should().BeCloseTo(DateTimeOffset.Now, 500);
        }
    }
}
