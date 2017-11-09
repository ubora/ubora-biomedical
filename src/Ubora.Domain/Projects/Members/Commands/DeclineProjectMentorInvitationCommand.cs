using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class DeclineProjectMentorInvitationCommand : UserCommand
    {
        public Guid InvitationId { get; set; }

        internal class Handler : ICommandHandler<DeclineProjectMentorInvitationCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeclineProjectMentorInvitationCommand cmd)
            {
                var invitation = _documentSession.LoadOrThrow<ProjectMentorInvitation>(cmd.InvitationId);

                invitation.Decline();
                _documentSession.Store(invitation);

                var notification = new NotificationToInviter(invitation.InvitedBy, invitation.InviteeUserId, invitation.ProjectId);
                _documentSession.Store(notification);

                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }

        public class NotificationToInviter : GeneralNotification
        {
            public NotificationToInviter(Guid notificationTo, Guid declinerUserId, Guid projectId) : base(notificationTo)
            {
                DeclinerUserId = declinerUserId;
                ProjectId = projectId;
            }

            public Guid DeclinerUserId { get; set; }
            public Guid ProjectId { get; set; }

            public override string GetDescription()
            {
                return $"{StringTokens.User(DeclinerUserId)} declined your invitation to join project {StringTokens.Project(ProjectId)} as mentor.";
            }
        }
    }
}