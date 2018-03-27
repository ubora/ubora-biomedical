using System;
using Marten.Events;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Notifications
{
    /// <summary>
    /// Notifies interested parties of commited <see cref="UboraEvent"/> through persisting <see cref="INotification"/> instances. 
    /// </summary>
    public abstract class EventNotifier<TEvent> : IEventHandler<TEvent> where TEvent : UboraEvent
    {
        public Type EventType => typeof(TEvent);
        
        public void Handle(IEvent eventWithMetadata)
        {
            HandleCore(@event: (TEvent)eventWithMetadata.Data, eventWithMetadata: eventWithMetadata);
        }

        protected abstract void HandleCore(TEvent @event, IEvent eventWithMetadata);
    }
}