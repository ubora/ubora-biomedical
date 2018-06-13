using FluentAssertions;
using System;
using TestStack.BDDfy;
using Ubora.Domain.Projects.Workpackages;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class RequestMentoringWorkpackageOneReviewCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();

        [Fact]
        public void Workpackage_Can_Be_Requested_Mentoring()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .When(_ => this.Request_Mentoring(_projectId))
                .Then(_ => this.Assert_Workpackage_One_Is_Requested_Mentoring())
                .BDDfy();
        }

        protected void Assert_Workpackage_One_Is_Requested_Mentoring()
        {
            var workpackageOne = Processor.FindById<WorkpackageOne>(_projectId);

            workpackageOne.HasBeenRequestedMentoring.Should().BeTrue();
        }
    }
}
