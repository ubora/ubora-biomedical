using FluentAssertions;
using System;
using System.Linq;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Commands;
using Ubora.Domain.Projects.Candidates.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Candidates.Commands
{
    public class AddCandidateCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _candidateId = Guid.NewGuid();
        private readonly string _candidateTitle = "title";
        private readonly string _candidateDescription = "description";
        private readonly BlobLocation _imageLocation = new BlobLocation("expectedContainer", "expectedBlobPath");

        [Fact]
        public void Adds_New_Candidate_To_Project()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .And(_ => this.Accept_Workpackage_One_Review(_projectId))
                .When(_ => Add_Candidate_To_Project())
                .Then(_ => Assert_Candidate_Is_Added_In_Project())
                .Then(_ => Assert_CandidateAdded_Is_Added_In_Events())
                .BDDfy();
        }

        private void Add_Candidate_To_Project()
        {
            var command = new AddCandidateCommand
            {
                ProjectId = _projectId,
                Id = _candidateId,
                Actor = new DummyUserInfo(),
                Title = _candidateTitle,
                Description = _candidateDescription,
                ImageLocation = _imageLocation
            };

            // Act
            Processor.Execute(command);
        }

        private void Assert_Candidate_Is_Added_In_Project()
        {
            var candidate = Session.Load<Candidate>(_candidateId);

            candidate.Id.Should().Be(_candidateId);
            candidate.ProjectId.Should().Be(_projectId);
            candidate.Title.Should().Be(_candidateTitle);
            candidate.Description.Should().Be(_candidateDescription);
            candidate.ImageLocation.Should().Be(_imageLocation);
        }

        private void Assert_CandidateAdded_Is_Added_In_Events()
        {
            var candidateAddedEvents = Session.Events.QueryRawEventDataOnly<CandidateAddedEvent>();

            candidateAddedEvents.Count().Should().Be(1);
            candidateAddedEvents.First().Id.Should().Be(_candidateId);
            candidateAddedEvents.First().ProjectId.Should().Be(_projectId);
            candidateAddedEvents.First().ImageLocation.BlobPath.Should().Be(_imageLocation.BlobPath);
        }
    }
}
