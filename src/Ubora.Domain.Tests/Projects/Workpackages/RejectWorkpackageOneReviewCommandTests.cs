using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.Workpackages;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class RejectWorkpackageOneReviewCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();

        [Fact]
        public void WorkpackageReviewCanBeRejected()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .When(_ => this.Reject_Workpackage_One_Review(_projectId))
                .Then(_ => this.Assert_Workpackage_One_Has_Rejected_Review())
                .BDDfy();
        }

        protected void Assert_Workpackage_One_Has_Rejected_Review()
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(_projectId);

            var review = workpackageOne.Reviews.Single();
            review.Status.Should().Be(WorkpackageReviewStatus.Rejected);
            review.ConcludedAt.Should().BeCloseTo(DateTimeOffset.Now, 500);
        }
    }
}
