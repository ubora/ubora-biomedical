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

        public Guid InvitedMemberId { get; private set; }
        public Guid ProjectId { get; private set; }
        public DateTime? DecidedAt { get; private set; }
        public bool? IsAccepted { get; private set; }
        public override bool IsArchived => !IsPending;
        public override bool IsPending => IsAccepted == null && DecidedAt == null;

        internal void Accept()
        {
            if (DecidedAt.HasValue) throw new InvalidOperationException("Already decided.");
            DecidedAt = DateTime.UtcNow;
            IsAccepted = true;
        }

        internal void Decline()
        {
            if (DecidedAt.HasValue) throw new InvalidOperationException("Already decided.");
            DecidedAt = DateTime.UtcNow;
            IsAccepted = false;
        }
    }
}
