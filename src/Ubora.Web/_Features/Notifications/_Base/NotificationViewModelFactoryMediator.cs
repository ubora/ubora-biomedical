using System;
using System.Collections.Generic;
using Ubora.Domain.Notifications;

namespace Ubora.Web._Features.Notifications._Base
{
    public class NotificationViewModelFactoryMediator
    {
        private readonly IEnumerable<INotificationViewModelFactory> _factories;

        /// <remarks> Autofac Implicit Relationship Types - http://docs.autofac.org/en/latest/resolve/relationships.html?highlight=ienumerable#enumeration-ienumerable-b-ilist-b-icollection-b </remarks>>
        public NotificationViewModelFactoryMediator(IEnumerable<INotificationViewModelFactory> factories)
        {
            _factories = factories;
        }

        public INotificationViewModel Create(BaseNotification notification)
        {
            var notificationType = notification.GetType();

            foreach (var factory in _factories)
            {
                if (factory.CanCreateFor(notificationType))
                {
                    var viewModel = ((dynamic)factory).Create((dynamic)notification);

                    return viewModel;
                }
            }

            throw new InvalidOperationException("View model factory not found for notification type.");
        }
    }
}