using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Candidates.Events;

namespace Ubora.Domain.Projects.Candidates.Commands
{
    public class EditCandidateCommentCommand : UserProjectCommand
    {
        public Guid CommentId { get; set; }
        public Guid CandidateId { get; set; }
        public string CommentText { get; set; }

        internal class Handler : CommandHandler<EditCandidateCommentCommand>
        {

            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(EditCandidateCommentCommand cmd)
            {
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var roleKeys = project.Members
                    .Where(m => m.UserId == cmd.Actor.UserId)
                    .Select(x => x.RoleKey)
                    .ToArray();

                var candidate = DocumentSession.LoadOrThrow<Candidate>(cmd.CandidateId);

                var @event = new CandidateCommentEditedEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    commentId: cmd.CommentId,
                    commentText: cmd.CommentText,
                    lastEditedAt: DateTime.UtcNow,
                    roleKeys: roleKeys
                );

                DocumentSession.Events.Append(cmd.CandidateId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
