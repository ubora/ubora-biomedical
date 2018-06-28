using FluentAssertions;
using System;
using System.Linq;
using TestStack.BDDfy;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Commands;
using Ubora.Domain.Projects.Candidates.Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Candidates.Commands
{
    public class RemoveCandidateCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _candidateId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();
        private ICommandResult _lastCommandResult;

        [Fact]
        public void Removes_Candidate_From_Votes()
        {
            this.Given(_ => Add_Candidate_To_Project(_candidateId))
                .And(_ => Add_Candidate_To_Project(Guid.NewGuid()))
                .When(_ => Remove_Candidate())
                .Then(_ => Assert_Candidate_Is_Removed())
                .Then(_ => Assert_CandidateRemoved_Is_Added_In_Events())
                .BDDfy();
        }

        private void Add_Candidate_To_Project(Guid candidateId)
        {
            new ProjectBuilder()
                .WithId(_projectId)
                .Build(this);

            var candidateAddedEvent = new CandidateAddedEvent(
                initiatedBy: new UserInfo(_userId, "username"),
                projectId: _projectId,
                id: candidateId,
                title: "title",
                description: "description",
                imageLocation: null);

            Session.Events.Append(_candidateId, candidateAddedEvent);
            Session.SaveChanges();
        }

        private void Remove_Candidate()
        {
            var command = new RemoveCandidateCommand
            {
                ProjectId = _projectId,
                Actor = new UserInfo(_userId, "username"),
                CandidateId = _candidateId
            };

            // Act
            _lastCommandResult = Processor.Execute(command);
        }

        private void Assert_Candidate_Is_Removed()
        {
            var candidates = Session.Query<Candidate>();

            candidates.Should().HaveCount(1);
        }

        private void Assert_CandidateRemoved_Is_Added_In_Events()
        {
            var candidateRemovedEvents = Session.Events.QueryRawEventDataOnly<CandidateRemovedEvent>();

            candidateRemovedEvents.Count().Should().Be(1);
            candidateRemovedEvents.First().InitiatedBy.UserId.Should().Be(_userId);
            candidateRemovedEvents.First().ProjectId.Should().Be(_projectId);
        }
    }
}
