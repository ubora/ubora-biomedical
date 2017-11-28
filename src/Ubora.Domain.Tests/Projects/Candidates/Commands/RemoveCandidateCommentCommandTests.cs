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
    public class RemoveCandidateCommentCommandTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _candidateId = Guid.NewGuid();
        private readonly Guid _commentId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();
        private readonly DateTime _commentedAt = DateTime.UtcNow;
        private ICommandResult _lastCommandResult;

        [Fact]
        public void Removes_Comment_From_Candidate()
        {
            this.Given(_ => Add_Candidate_To_Project())
                .And(_ => Add_Comment_To_Candidate())
                .When(_ => Remove_Candidate_Comment())
                .Then(_ => Assert_Comment_Is_Removed())
                .Then(_ => Assert_CandidateCommentRemoved_Is_Added_In_Events())
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

            Session.Events.Append(_candidateId, candidateAddedEvent);
            Session.SaveChanges();
        }

        private void Add_Comment_To_Candidate()
        {
            var commentAddedEvent = new CandidateCommentAddedEvent(
                initiatedBy: new UserInfo(_userId, "username"),
                projectId: _projectId,
                commentId: _commentId,
                commentText: "comment",
                commentedAt: _commentedAt,
                roleKeys: new[] { "project-member", "project-leader" }
                );

            Session.Events.Append(_candidateId, commentAddedEvent);
            Session.SaveChanges();
        }

        private void Remove_Candidate_Comment()
        {
            var command = new RemoveCandidateCommentCommand
            {
                ProjectId = _projectId,
                Actor = new UserInfo(_userId, "username"),
                CandidateId = _candidateId,
                CommentId = _commentId
            };

            // Act
            _lastCommandResult = Processor.Execute(command);
        }

        private void Assert_Comment_Is_Removed()
        {
            var candidate = Session.Load<Candidate>(_candidateId);

            var commentsCount = candidate.Comments.Count;
            commentsCount.Should().Be(0);

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }

        private void Assert_CandidateCommentRemoved_Is_Added_In_Events()
        {
            var candidateCommentRemovedEvents = Session.Events.QueryRawEventDataOnly<CandidateCommentRemovedEvent>();

            candidateCommentRemovedEvents.Count().Should().Be(1);
            candidateCommentRemovedEvents.First().InitiatedBy.UserId.Should().Be(_userId);
            candidateCommentRemovedEvents.First().ProjectId.Should().Be(_projectId);
            candidateCommentRemovedEvents.First().CommentId.Should().Be(_commentId);
        }
    }
}
