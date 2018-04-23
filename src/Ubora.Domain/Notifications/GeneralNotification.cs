using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Notifications
{
    public abstract class GeneralNotification : INotification
    {
        protected GeneralNotification(Guid notificationTo)
        {
            Id = Guid.NewGuid();
            NotificationTo = notificationTo;
            CreatedAt = DateTime.UtcNow;
        }

        [JsonConstructor]
        protected GeneralNotification(Guid id, Guid notificationTo, DateTime createdAt)
        {
            Id = id;
            NotificationTo = notificationTo;
            CreatedAt = createdAt;
        }

        public Guid Id { get; }
        public Guid NotificationTo { get; }
        public DateTime CreatedAt { get; }
        public bool HasBeenViewed { get; set; }
        public bool IsArchived => HasBeenViewed;

        public abstract string GetDescription();
    }
}