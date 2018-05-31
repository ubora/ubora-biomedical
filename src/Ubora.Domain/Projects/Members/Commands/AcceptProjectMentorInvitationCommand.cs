using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class AcceptProjectMentorInvitationCommand : UserCommand
    {
        public Guid InvitationId { get; set; }

        internal class Handler : ICommandHandler<AcceptProjectMentorInvitationCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AcceptProjectMentorInvitationCommand command)
            {
                var invitation = _documentSession.LoadOrThrow<ProjectMentorInvitation>(command.InvitationId);

                invitation.Accept();

                var workpackageOne = _documentSession.LoadOrThrow<WorkpackageOne>(invitation.ProjectId);
                workpackageOne.HasBeenRequestedMentoring = false;
                _documentSession.Store(workpackageOne);

                var @event = new MentorJoinedProjectEvent(
                    projectId: invitation.ProjectId,
                    userId: invitation.InviteeUserId,
                    initiatedBy: command.Actor);

                _documentSession.Events.Append(invitation.ProjectId, @event);
                _documentSession.Store(invitation);

                var notification = new MentorJoinedProjectEvent.NotificationToInviter(invitation.InvitedBy, invitation.InviteeUserId, invitation.ProjectId);

                _documentSession.Store(notification);

                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
