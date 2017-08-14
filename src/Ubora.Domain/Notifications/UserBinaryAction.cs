using System;

namespace Ubora.Domain.Notifications
{
    public abstract class UserBinaryAction : INotification
    {
        public Guid Id { get; private set; }
        public Guid NotificationTo { get; private set; }
        public bool HasBeenViewed { get; internal set; }
        public abstract bool IsArchived { get; }
        public abstract bool IsPending { get; }
        public DateTime? DecidedAt { get; private set; }
        public bool? IsAccepted { get; private set; }

        protected UserBinaryAction(Guid userId)
        {
            Id = Guid.NewGuid();
            NotificationTo = userId;
        }

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