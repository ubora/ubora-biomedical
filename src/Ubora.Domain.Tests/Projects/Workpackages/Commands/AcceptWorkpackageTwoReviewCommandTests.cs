using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.Workpackages;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class AcceptWorkpackageTwoReviewCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();

        [Fact]
        public void Workpackage_Two_Review_Can_Be_Accepted()
        {
            this.Given(_ => this.Create_Project(_projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                    .And(_ => this.Accept_Workpackage_One_Review(_projectId))
                    .And(_ => this.Submit_Workpackage_Two_For_Review(_projectId))
                .When(_ => this.Accept_Workpackage_Two_Review(_projectId))
                .Then(_ => this.Workpackage_Two_Should_Have_Accepted_Review())
                .BDDfy();
        }


        protected void Workpackage_Two_Should_Have_Accepted_Review()
        {
            var workpackageTwo = Processor.FindById<WorkpackageTwo>(_projectId);

            var review = workpackageTwo.Reviews.Single();

            review.Status.Should().Be(WorkpackageReviewStatus.Accepted);
            review.ConcludedAt.Should().BeCloseTo(DateTimeOffset.Now, 500);
        }
    }
}
