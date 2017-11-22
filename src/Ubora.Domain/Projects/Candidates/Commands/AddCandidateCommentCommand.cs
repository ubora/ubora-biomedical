using Marten;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Projects.Candidates.Events;


namespace Ubora.Domain.Projects.Candidates.Commands
{
    public class AddCandidateCommentCommand : UserProjectCommand
    {
        public Guid CandidateId { get; set; }
        public string CommentText { get; set; }

        internal class Handler : CommandHandler<AddCandidateCommentCommand>
        {

            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AddCandidateCommentCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var roleKeys = project.Members
                    .Where(m => m.UserId == cmd.Actor.UserId)
                    .Select(x => x.RoleKey)
                    .ToArray();

                var candidate = DocumentSession.LoadOrThrow<Candidate>(cmd.CandidateId);

                var @event = new CandidateCommentAddedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    commentText: cmd.CommentText,
                    commentId: Guid.NewGuid(),
                    commentedAt: DateTime.UtcNow,
                    roleKeys: roleKeys
                );

                DocumentSession.Events.Append(cmd.CandidateId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
