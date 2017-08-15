using System;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects.Members
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