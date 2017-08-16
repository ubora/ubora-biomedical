using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class DeclineInvitationToJoinProjectAsMentorCommand : UserCommand
    {
        public Guid InvitationId { get; set; }

        internal class Handler : ICommandHandler<DeclineInvitationToJoinProjectAsMentorCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeclineInvitationToJoinProjectAsMentorCommand cmd)
            {
                var invite = _documentSession.LoadOrThrow<ProjectMentorInvitation>(cmd.InvitationId);

                invite.Decline();
                _documentSession.Store(invite);

                var notification = new YourInvitationToJoinProjectAsMentorWasDeclinedNotification(invite.InvitedBy, invite.InviteeUserId, invite.ProjectId);
                _documentSession.Store(notification);

                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }

        public class YourInvitationToJoinProjectAsMentorWasDeclinedNotification : GeneralNotification
        {
            public YourInvitationToJoinProjectAsMentorWasDeclinedNotification(Guid notificationTo, Guid declinerUserId, Guid projectId) : base(notificationTo)
            {
                DeclinerUserId = declinerUserId;
                ProjectId = projectId;
            }

            public Guid DeclinerUserId { get; set; }
            public Guid ProjectId { get; set; }

            public override string GetDescription()
            {
                return $"{Template.User(DeclinerUserId)} declined your invitation to join project {Template.Project(ProjectId)} as mentor.";
            }
        }
    }
}