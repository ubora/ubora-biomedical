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
    public class AddCandidateCommentCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _candidateId = Guid.NewGuid();
        private readonly string _comment = "comment";
        private readonly Guid _userId = Guid.NewGuid();
        private ICommandResult _lastCommandResult;

        [Fact]
        public void Adds_New_Comment_To_Candidate()
        {
            this.Given(_ => Add_Candidate_To_Project())
                .When(_ => Add_Comment_To_Candidate())
                .Then(_ => Assert_Comment_Is_Added_To_Candidate())
                .Then(_ => Assert_CandidateCommentAdded_Is_Added_In_Events())
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

        private void Add_Comment_To_Candidate()
        {
            var command = new AddCandidateCommentCommand
            {
                ProjectId = _projectId,
                Actor = new UserInfo(_userId, "username"),
                CommentText = _comment,
                CandidateId = _candidateId
            };

            // Act
            _lastCommandResult = Processor.Execute(command);
        }

        private void Assert_Comment_Is_Added_To_Candidate()
        {
            var candidate = Session.Load<Candidate>(_candidateId);

            var addedComment = candidate.Comments.Last();
            addedComment.UserId.Should().Be(_userId);
            addedComment.Text.Should().Be(_comment);

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }

        private void Assert_CandidateCommentAdded_Is_Added_In_Events()
        {
            var candidateCommentAddedEvents = Session.Events.QueryRawEventDataOnly<CandidateCommentAddedEvent>();

            candidateCommentAddedEvents.Count().Should().Be(1);
            candidateCommentAddedEvents.First().InitiatedBy.UserId.Should().Be(_userId);
            candidateCommentAddedEvents.First().ProjectId.Should().Be(_projectId);
            candidateCommentAddedEvents.First().CommentText.Should().Be(_comment);
        }
    }
}
