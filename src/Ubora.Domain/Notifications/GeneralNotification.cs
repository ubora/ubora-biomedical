using System;

namespace Ubora.Domain.Notifications
{
    public abstract class GeneralNotification : INotification
    {
        protected GeneralNotification(Guid notificationTo)
        {
            Id = Guid.NewGuid();
            NotificationTo = notificationTo;
        }

        public Guid Id { get; private set; }
        public Guid NotificationTo { get; private set; }
        public bool HasBeenViewed { get; set; }
        public bool IsArchived => HasBeenViewed;

        public abstract string GetDescription();
    }
}