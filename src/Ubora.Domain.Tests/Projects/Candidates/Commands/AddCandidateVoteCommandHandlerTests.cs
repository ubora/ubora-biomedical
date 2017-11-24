using System;
using System.Linq;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Commands;
using Ubora.Domain.Projects.Candidates.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Candidates.Commands
{
    public class AddCandidateVoteCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _candidateId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();
        private readonly int _functionalityScore = 4;
        private readonly int _performanceScore = 2;
        private readonly int _usabilityScore = 3;
        private readonly int _safetyScore = 1;
        private ICommandResult _lastCommandResult;

        [Fact]
        public void Adds_New_Vote_To_Candidate()
        {
            this.Given(_ => this.Create_User(_userId, "email", "firstName", "lastName"))
                .Given(_ => this.Create_Project(_projectId, _userId))
                .Given(_ => Add_Candidate_To_Project())
                .When(_ => Add_Vote_To_Candidate())
                .Then(_ => Assert_Vote_Is_Added_To_Candidate())
                .Then(_ => Assert_CandidateVoteAdded_Is_Added_In_Events())
                .BDDfy();
        }

        private void Add_Candidate_To_Project()
        {
            var candidateAddedEvent = new CandidateAddedEvent(
                initiatedBy: new UserInfo(_userId, "username"),
                projectId: _projectId,
                id: _candidateId,
                title: "title",
                description: "description",
                imageLocation: null);

            Session.Events.Append(_projectId, candidateAddedEvent);
            Session.SaveChanges();
        }

        private void Add_Vote_To_Candidate()
        {
            var command = new AddCandidateVoteCommand
            {
                ProjectId = _projectId,
                CandidateId = _candidateId,
                Actor = new UserInfo(_userId, "username"),
                Usability = _usabilityScore,
                Functionality = _functionalityScore,
                Safety = _safetyScore,
                Performance = _performanceScore
            };

            // Act
            _lastCommandResult = Processor.Execute(command);
        }

        private void Assert_Vote_Is_Added_To_Candidate()
        {
            var candidate = Session.Load<Candidate>(_candidateId);

            var addedVote = candidate.Votes.Last();
            addedVote.UserId.Should().Be(_userId);
            addedVote.Safety.Should().Be(_safetyScore);
            addedVote.Functionality.Should().Be(_functionalityScore);
            addedVote.Performance.Should().Be(_performanceScore);
            addedVote.Usability.Should().Be(_usabilityScore);
            addedVote.Score.Should().Be(10);
            _lastCommandResult.IsSuccess.Should().BeTrue();
        }

        private void Assert_CandidateVoteAdded_Is_Added_In_Events()
        {
            var candidateVoteAddedEvent = Session.Events.QueryRawEventDataOnly<CandidateVoteAddedEvent>();

            candidateVoteAddedEvent.Count().Should().Be(1);
            candidateVoteAddedEvent.First().InitiatedBy.UserId.Should().Be(_userId);
            candidateVoteAddedEvent.First().ProjectId.Should().Be(_projectId);
            candidateVoteAddedEvent.First().CandidateId.Should().Be(_candidateId);
            candidateVoteAddedEvent.First().Functionality.Should().Be(_functionalityScore);
            candidateVoteAddedEvent.First().Perfomance.Should().Be(_performanceScore);
            candidateVoteAddedEvent.First().Usability.Should().Be(_usabilityScore);
            candidateVoteAddedEvent.First().Safety.Should().Be(_safetyScore);
        }
    }
}
