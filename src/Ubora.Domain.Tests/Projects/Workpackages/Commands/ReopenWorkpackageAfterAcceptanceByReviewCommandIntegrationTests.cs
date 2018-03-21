using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Commands;
using Ubora.Domain.Projects.Workpackages.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class ReopenWorkpackageAfterAcceptanceByReviewCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void WP1_Can_Be_Reopened_After_Acceptance_By_Review()
        {
            var projectId = Guid.NewGuid();

            this.Given(_ => this.Create_Project(projectId))
                    .And(_ => this.Submit_Workpackage_One_For_Review(projectId))
                    .And(_ => this.Accept_Workpackage_One_Review(projectId))
                .When(_ => Execute_Command_For_Reopening_WP1(projectId))
                .Then(_ => Assert_Event_Is_Persisted(projectId))
                    .And(_ => Previously_Accepted_Review_Status_Should_Be_Changed(projectId))
                    .And(_ => WP1_Should_Be_Opened_Again(projectId))
                    .And(_ => Project_Should_Be_Draft_Again(projectId))
                .BDDfy();
        }

        private void Execute_Command_For_Reopening_WP1(Guid projectId)
        {
            var wp1 = Processor.FindById<WorkpackageOne>(projectId);
            var acceptedReviewId = wp1.GetLatestReviewOrNull().Id;

            var command = new ReopenWorkpackageAfterAcceptanceByReviewCommand
            {
                ProjectId = projectId,
                LatestReviewId = acceptedReviewId,
                Actor = new DummyUserInfo()
            };
            var result = Processor.Execute(command);
            result.IsSuccess.Should().BeTrue();
        }

        private void Previously_Accepted_Review_Status_Should_Be_Changed(Guid projectId)
        {
            var wp1 = Processor.FindById<WorkpackageOne>(projectId);
            var previouslyAcceptedReview = wp1.GetLatestReviewOrNull();
            previouslyAcceptedReview.Status.Should().Be(WorkpackageReviewStatus.AcceptedButWorkpackageReopened);
        }

        private void WP1_Should_Be_Opened_Again(Guid projectId)
        {
            var wp1 = Processor.FindById<WorkpackageOne>(projectId);
            wp1.IsLocked.Should().BeFalse();
        }

        private void Assert_Event_Is_Persisted(Guid projectId)
        {
            var wp1 = Processor.FindById<WorkpackageOne>(projectId);
            var previouslyAcceptedReview = wp1.GetLatestReviewOrNull();

            var expectedEvent = Session.Events.FetchStream(projectId).Select(x => x.Data).Last() as WorkpackageOneReopenedAfterAcceptanceByReviewEvent;

            expectedEvent.Should().NotBeNull();
            expectedEvent.AcceptedReviewId.Should().Be(previouslyAcceptedReview.Id);
            expectedEvent.ProjectId.Should().Be(projectId);
        }

        private void Project_Should_Be_Draft_Again(Guid projectId)
        {
            var project = Processor.FindById<Project>(projectId);
            project.IsInDraft.Should().BeTrue();
        }
    }
}
