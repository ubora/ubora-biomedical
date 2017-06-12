using Marten.Schema;
using System;

namespace Ubora.Domain.Notifications
{
    public class InvitationToProject
    {
        public InvitationToProject(Guid id, Guid invitedMemberId, Guid projectId)
        {
            Id = id;
            InvitedMemberId = invitedMemberId;
            ProjectId = projectId;
        }

        [Identity]
        public Guid Id { get; }
        public Guid InvitedMemberId { get; }
        public Guid ProjectId { get; }
        public bool HasBeenViewed { get; internal set; }
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
    }
}
