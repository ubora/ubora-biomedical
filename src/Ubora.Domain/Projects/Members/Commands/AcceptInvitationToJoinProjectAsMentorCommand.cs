using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class AcceptInvitationToJoinProjectAsMentorCommand : UserCommand
    {
        public Guid InvitationId { get; set; }

        internal class Handler : ICommandHandler<AcceptInvitationToJoinProjectAsMentorCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            // TODO(tests)
            public ICommandResult Handle(AcceptInvitationToJoinProjectAsMentorCommand command)
            {
                var invite = _documentSession.LoadOrThrow<ProjectMentorInvitation>(command.InvitationId);
                var userProfile = _documentSession.LoadOrThrow<UserProfile>(invite.InviteeUserId);
                var project = _documentSession.LoadOrThrow<Project>(invite.ProjectId);

                invite.Accept();

                var @event = new MentorJoinedProjectEvent(
                    projectId: project.Id,
                    userId: userProfile.UserId,
                    userFullName: userProfile.FullName,
                    initiatedBy: command.Actor);

                _documentSession.Events.Append(invite.ProjectId, @event);
                _documentSession.Store(invite);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
