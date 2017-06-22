using System;

namespace Ubora.Domain.Notifications.Invitation
{
    public class InvitationToProject : BaseNotification
    {
        public InvitationToProject(Guid id, Guid inviteTo, Guid invitedMemberId, Guid projectId) : base(id, inviteTo)
        {
            InvitedMemberId = invitedMemberId;
            ProjectId = projectId;
        }

        public Guid InvitedMemberId { get; }
        public Guid ProjectId { get; }
        public bool? IsAccepted
        {
            get
            {
                if (Accepted != null && Declined == null)
                {
                    return true;
                }
                if (Declined != null && Accepted == null)
                {
                    return false;
                }

                return null;
            }
        }
        public DateTime? Accepted { get; internal set; }
        public DateTime? Declined { get; internal set; }
        public override bool IsArchived => !IsPending;
        public override bool IsPending => Accepted == null && Declined == null;
    }
}
