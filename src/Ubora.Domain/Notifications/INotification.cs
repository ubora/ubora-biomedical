using System;

namespace Ubora.Domain.Notifications
{
    public interface INotification
    {
        Guid Id { get; }
        /// <summary>
        /// UserId of the user the notification is to.
        /// </summary>
        Guid NotificationTo { get; }
        bool HasBeenViewed { get; }
        bool IsArchived { get; }
    }
}