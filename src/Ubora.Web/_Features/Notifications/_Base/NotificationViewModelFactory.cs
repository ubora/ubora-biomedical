using System;
using Ubora.Domain.Notifications;

namespace Ubora.Web._Features.Notifications._Base
{
    public abstract class NotificationViewModelFactory<TNotification, TViewModel> : INotificationViewModelFactory
        where TNotification : INotification
        where TViewModel : INotificationViewModel<TNotification>
    {
        public bool CanCreateFor(Type type)
        {
            return type == typeof(TNotification);
        }

        public abstract TViewModel Create(TNotification notification);
    }
}