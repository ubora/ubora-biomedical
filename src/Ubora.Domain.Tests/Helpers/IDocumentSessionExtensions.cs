using System;

// ReSharper disable once CheckNamespace
namespace Marten
{
    // ReSharper disable once InconsistentNaming
    public static class IDocumentSessionExtensions
    {
        public static void EventsAppendAndSave(this IDocumentSession @this, Guid aggregateId, params object[] events)
        {
            @this.Events.Append(aggregateId, events);
            @this.SaveChanges();
        }

        public static TAggregate EventsAppendAndSaveAndLoad<TAggregate>(this IDocumentSession @this, Guid aggregateId, params object[] events)
        {
            @this.Events.Append(aggregateId, events);
            @this.SaveChanges();
            return @this.Load<TAggregate>(aggregateId);
        }
    }
}
