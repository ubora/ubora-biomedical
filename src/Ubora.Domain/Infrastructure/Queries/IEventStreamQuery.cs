using System;
using System.Collections.Generic;
using System.Linq;
using Marten.Events;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Repository.Events;

namespace Ubora.Domain.Infrastructure.Queries
{
    public interface IEventStreamQuery
    {
        IEnumerable<UboraEvent> Find(Guid streamId);
        IEnumerable<IEvent> FindFileEvents(Guid streamId, Guid fileId);
        IEvent FindFileEvent(Guid streamId, Guid eventId);
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

        public IEnumerable<IEvent> FindFileEvents(Guid streamId, Guid fileId)
        {
            var uboraEventStream = _eventStore.FetchStream(streamId);

            var fileEvents = uboraEventStream.Where(x => x.Data is UboraFileEvent && ((UboraFileEvent)x.Data).Id == fileId);

            return fileEvents;
        }

        public IEvent FindFileEvent(Guid streamId, Guid eventId)
        {
            var uboraEventStream = _eventStore.FetchStream(streamId);

            var fileEvent = uboraEventStream.Single(x => x.Data is UboraFileEvent && x.Id == eventId);

            return fileEvent;
        }
    }
}