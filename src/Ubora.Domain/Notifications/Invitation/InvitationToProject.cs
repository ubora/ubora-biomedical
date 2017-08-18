using System;

namespace Ubora.Domain.Notifications.Invitation
{
    public class InvitationToProject : UserBinaryAction
    {
        public InvitationToProject(Guid inviteTo, Guid projectId) : base(inviteTo)
        {
            ProjectId = projectId;
            InvitedMemberId = inviteTo;
        }

        public Guid InvitedMemberId { get; private set; }
        public Guid ProjectId { get; private set; }
    }
}