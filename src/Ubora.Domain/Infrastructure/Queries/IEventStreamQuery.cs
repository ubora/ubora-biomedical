using System;
using System.Collections.Generic;
using System.Linq;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Infrastructure.Queries
{
    public interface IEventStreamQuery
    {
        IEnumerable<UboraEvent> Find(Guid streamId);
    }

    public class EventStreamQuery : IEventStreamQuery
    {
        private readonly IEventStore _eventStore;

        public EventStreamQuery(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public IEnumerable<UboraEvent> Find(Guid streamId)
        {
            var uboraEventStream = _eventStore.FetchStream(streamId);

            var uboraEvents = Enumerable.Select<IEvent, UboraEvent>(uboraEventStream, x => (UboraEvent)x.Data);

            return uboraEvents;
        }
    }
}