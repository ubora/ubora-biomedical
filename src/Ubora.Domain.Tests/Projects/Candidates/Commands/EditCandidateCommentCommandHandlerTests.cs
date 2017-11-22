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
    public class EditCandidateCommentCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();
        private readonly Guid _candidateId = Guid.NewGuid();
        private readonly Guid _commentId = Guid.NewGuid();
        private readonly string _editedComment = "editedComment";
        private readonly Guid _userId = Guid.NewGuid();
        private readonly DateTime _commentedAt = DateTime.UtcNow;
        private ICommandResult _lastCommandResult;

        [Fact]
        public void Edits_Candidate_Comment()
        {
            this.Given(_ => this.Create_User(_userId, "email", "firstName", "lastName"))
                .And(_ => this.Create_Project(_projectId, _userId))
                .And(_ => Add_Candidate_To_Project())
                .And(_ => Add_Comment_To_Candidate())
                .And(_ => this.Assign_Project_Mentor(_projectId, _userId))
                .When(_ => Edit_Candidate_Comment())
                .Then(_ => Assert_Comment_Is_Edited())
                .Then(_ => Assert_CandidateCommentEdited_Is_Added_In_Events())
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
                roleKeys: new []{ "project-member", "project-leader" }
                );

            Session.Events.Append(_candidateId, commentAddedEvent);
            Session.SaveChanges();
        }

        private void Edit_Candidate_Comment()
        {
            var command = new EditCandidateCommentCommand
            {
                ProjectId = _projectId,
                Actor = new UserInfo(_userId, "username"),
                CommentText = _editedComment,
                CandidateId = _candidateId,
                CommentId = _commentId
            };

            // Act
            _lastCommandResult = Processor.Execute(command);
        }

        private void Assert_Comment_Is_Edited()
        {
            var candidate = Session.Load<Candidate>(_candidateId);

            var editedComment = candidate.Comments.Single();
            editedComment.UserId.Should().Be(_userId);
            editedComment.Text.Should().Be(_editedComment);
            editedComment.CommentedAt.Should().Be(_commentedAt);
            editedComment.Id.Should().Be(_commentId);
            editedComment.RoleKeys.Should().BeEquivalentTo(new[] { "project-leader", "project-mentor" });

            _lastCommandResult.IsSuccess.Should().BeTrue();
        }

        private void Assert_CandidateCommentEdited_Is_Added_In_Events()
        {
            var candidateCommentEditedEvents = Session.Events.QueryRawEventDataOnly<CandidateCommentEditedEvent>();

            candidateCommentEditedEvents.Count().Should().Be(1);
            candidateCommentEditedEvents.First().InitiatedBy.UserId.Should().Be(_userId);
            candidateCommentEditedEvents.First().ProjectId.Should().Be(_projectId);
            candidateCommentEditedEvents.First().CommentText.Should().Be(_editedComment);
            candidateCommentEditedEvents.First().CommentId.Should().Be(_commentId);
            candidateCommentEditedEvents.First().RoleKeys.Should().BeEquivalentTo(new[] { "project-leader", "project-mentor" });
        }
    }
}
