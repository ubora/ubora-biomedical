using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Notifications;

// ReSharper disable once CheckNamespace
namespace Marten
{
    public static class DocumentSessionExtensions
    {
        public static T LoadOrThrow<T>(this IDocumentSession documentSession, Guid id)
        {
            var loaded = documentSession.Load<T>(id);
            if (loaded == null)
            {
                throw new InvalidOperationException($"{typeof(T).Name} not found with ID: {id}");
            }
            return loaded;
        }
        
        public static void StoreUboraNotificationsIfAny(this IDocumentSession documentSession, IEnumerable<INotification> notifications)
        {
            if (notifications == null)
            {
                return;
            }

            var notificationArray = notifications.ToArray();
            if (notificationArray.Any())
            {
                // ReSharper disable once RedundantTypeArgumentsOfMethod
                // ReSharper disable once ArgumentsStyleOther
                documentSession.Store<INotification>(entities: notificationArray);
            }
        }
    }
}