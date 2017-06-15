using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages
{
    public class RejectWorkpackageOneReviewCommandTests : IntegrationFixture
    {
        [Fact]
        public void WorkpackageReviewCanBeRejected()
        {
            var projectId = Guid.NewGuid();

            this.Given(_ => this.Create_Project(projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(projectId))
                .When(_ => this.Reject_Workpackage_One_Review(projectId))
                .Then(_ => this.Assert_Workpackage_One_Has_Review_In_Status(projectId, WorkpackageReviewStatus.Rejected))
                .BDDfy();
        }

        protected void Assert_Workpackage_One_Has_Review_In_Status(Guid workpackageId, WorkpackageReviewStatus status)
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(workpackageId);

            var isAccepted = workpackageOne.DoesSatisfy(new HasReviewInStatus<WorkpackageOne>(status));
            isAccepted.Should().BeTrue();
        }
    }
}
