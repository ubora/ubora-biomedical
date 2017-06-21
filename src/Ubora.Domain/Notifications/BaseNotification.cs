using Marten.Schema;
using System;

namespace Ubora.Domain.Notifications
{
    public abstract class BaseNotification
    {
        [Identity]
        public Guid Id { get; }
        public Guid NotificationTo { get; }
        public bool HasBeenViewed { get; internal set; }
        public abstract bool InHistory { get; }

        public BaseNotification(Guid id, Guid inviteTo)
        {
            Id = id;
            NotificationTo = inviteTo;
        }
    }
}
