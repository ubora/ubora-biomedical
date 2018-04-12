using Marten.Events;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Infrastructure
{
    public abstract class UboraEventHandler<TEvent> where TEvent : UboraEvent
    {
        public void Handle(IEvent eventWithMetadata)
        {
            HandleCore(@event: (TEvent)eventWithMetadata.Data, eventWithMetadata: eventWithMetadata);
        }

        /// <remarks>
        /// WARNING: IEvent might not have all metadata fields initialized at this point. Event ID is for certain though. 
        /// </remarks>>
        protected abstract void HandleCore(TEvent @event, IEvent eventWithMetadata);
    }
}