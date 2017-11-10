using Marten.Events;
using Marten.Events.Projections;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.History
{
    public class EventToHistoryTransformer<TEvent> : ITransform<TEvent, EventLogEntry> where TEvent : ProjectEvent
    {
        public EventLogEntry Transform(EventStream stream, Event<TEvent> input)
        {
            return EventLogEntry.FromEvent(input);
        }
    }
}
