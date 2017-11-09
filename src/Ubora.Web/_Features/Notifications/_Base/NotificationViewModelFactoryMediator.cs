using System;
using System.Collections.Generic;
using Ubora.Domain.Notifications;

namespace Ubora.Web._Features.Notifications._Base
{
    public class NotificationViewModelFactoryMediator
    {
        private readonly IEnumerable<INotificationViewModelFactory> _concreteFactories;
        private readonly GeneralNotificationViewModel.Factory _generalFactory;

        /// <remarks> Autofac Implicit Relationship Types - http://docs.autofac.org/en/latest/resolve/relationships.html?highlight=ienumerable#enumeration-ienumerable-b-ilist-b-icollection-b </remarks>>
        public NotificationViewModelFactoryMediator(IEnumerable<INotificationViewModelFactory> concreteFactories, GeneralNotificationViewModel.Factory generalFactory)
        {
            _generalFactory = generalFactory;
            _concreteFactories = concreteFactories;
        }

        protected NotificationViewModelFactoryMediator()
        {
        }

        public virtual INotificationViewModel Create(INotification notification)
        {
            var notificationType = notification.GetType();

            foreach (var factory in _concreteFactories)
            {
                if (factory.CanCreateFor(notificationType))
                {
                    var viewModel = ((dynamic)factory).Create((dynamic)notification);

                    return viewModel;
                }
            }

            if (_generalFactory.CanCreateFor(notificationType))
            {
                return _generalFactory.Create((GeneralNotification)notification);
            }

            throw new InvalidOperationException("View model factory not found for notification type.");
        }
    }
}