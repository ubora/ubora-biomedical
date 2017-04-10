using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Marten.Events;
using Ubora.Domain.Events;

namespace Ubora.Domain.Queries
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

            var uboraEvents = uboraEventStream.Select(x => (UboraEvent)x.Data);

            return uboraEvents;
        }
    }
}