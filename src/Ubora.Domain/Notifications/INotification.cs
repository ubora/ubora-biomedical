using System;

namespace Ubora.Domain.Notifications
{
    // TODO: Don't forget to add type to AddSubClassHierarchy() in UboraStoreOptions
    public interface INotification
    {
        Guid Id { get; }
        /// <summary>
        /// UserId of the user the notification is to.
        /// </summary>
        Guid NotificationTo { get; }
        // Todo: internal
        bool HasBeenViewed { get; set; }
        bool IsArchived { get; }
    }
}