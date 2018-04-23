using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Notifications
{
    public abstract class UserBinaryAction : INotification
    {
        public Guid Id { get; }
        public Guid NotificationTo { get; }
        public DateTime CreatedAt { get; }
        public bool HasBeenViewed { get; set; }
        public DateTime? DecidedAt { get; private set; }
        public bool? IsAccepted { get; private set; }
        public bool IsArchived => !IsPending;
        public bool IsPending => IsAccepted == null && DecidedAt == null;

        protected UserBinaryAction(Guid userId)
        {
            Id = Guid.NewGuid();
            NotificationTo = userId;
            CreatedAt = DateTime.UtcNow;
        }

        [JsonConstructor]
        protected UserBinaryAction(Guid id, Guid notificationTo, DateTime createdAt)
        {
            Id = id;
            NotificationTo = notificationTo;
            CreatedAt = createdAt;
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