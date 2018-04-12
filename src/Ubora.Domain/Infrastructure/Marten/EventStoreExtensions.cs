using System.Linq;
using Marten.Events;

// ReSharper disable once CheckNamespace
namespace Marten
{
    public static class EventStoreExtensions
    {
        public static IEvent FindLastEvent(this IEventStore eventStore)
        {
            return eventStore
                .QueryAllRawEvents()
                .OrderByDescending(@event => @event.Timestamp)
                .FirstOrDefault();
        }
    }
}