using System;

namespace Ubora.Domain.Notifications.Join
{
    public class RequestToJoinProject : BaseNotification
    {
        public RequestToJoinProject(Guid id, Guid inviteTo, Guid askingToJoinMemberId, Guid projectId) : base(id, inviteTo)
        {
            AskingToJoinMemberId = askingToJoinMemberId;
            ProjectId = projectId;
        }

        public Guid AskingToJoinMemberId { get; private set; }
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
