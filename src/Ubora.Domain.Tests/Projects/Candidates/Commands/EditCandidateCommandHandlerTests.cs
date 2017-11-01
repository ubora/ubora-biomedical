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
    public class EditCandidateCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _candidateId = Guid.NewGuid();
        private readonly string _candidateTitle = "updatedTitle";
        private readonly string _candidateDescription = "updatedDescription";
        private readonly BlobLocation _imageLocation = new BlobLocation("expectedContainer", "expectedBlobPath");

        [Fact]
        public void Edits_Project_Candidate()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .And(_ => Add_Candidate_To_Project())
                .When(_ => Update_Candidate())
                .Then(_ => Assert_Candidate_Is_Updated())
                .Then(_ => Assert_CandidateEdited_Is_Added_In_Events())
                .BDDfy();
        }

        private void Add_Candidate_To_Project()
        {
            var candidateAddedEvent = new CandidateAddedEvent(
                initiatedBy: new DummyUserInfo(),
                projectId: _projectId,
                id: _candidateId,
                title: "title",
                description: "description",
                imageLocation: _imageLocation);

            Session.Events.Append(_projectId, candidateAddedEvent);
            Session.SaveChanges();
        }

        private void Update_Candidate()
        {
            var editCandidateCommand = new EditCandidateCommand
            {
                Actor = new DummyUserInfo(),
                ProjectId = _projectId,
                Id = _candidateId,
                Title = _candidateTitle,
                Description = _candidateDescription
            };

            Processor.Execute(editCandidateCommand);
        }

        private void Assert_Candidate_Is_Updated()
        {
            var candidate = Session.Load<Candidate>(_candidateId);

            candidate.Id.Should().Be(_candidateId);
            candidate.ProjectId.Should().Be(_projectId);
            candidate.Title.Should().Be(_candidateTitle);
            candidate.Description.Should().Be(_candidateDescription);
            candidate.ImageLocation.Should().Be(_imageLocation);
        }

        private void Assert_CandidateEdited_Is_Added_In_Events()
        {
            var candidateEditedEvents = Session.Events.QueryRawEventDataOnly<CandidateEditedEvent>();

            candidateEditedEvents.Count().Should().Be(1);
            candidateEditedEvents.First().Id.Should().Be(_candidateId);
            candidateEditedEvents.First().ProjectId.Should().Be(_projectId);
            candidateEditedEvents.First().Title.Should().Be(_candidateTitle);
            candidateEditedEvents.First().Description.Should().Be(_candidateDescription);
        }
    }
}
