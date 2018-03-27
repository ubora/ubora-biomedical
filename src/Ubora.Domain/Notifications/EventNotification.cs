using System;
using Marten.Events;
using Newtonsoft.Json;

namespace Ubora.Domain.Notifications
{
    public class EventNotification : INotification
    {
        [JsonConstructor]
        protected EventNotification(Guid id, Type eventType, Guid eventId, Guid notificationTo)
        {
            Id = id;
            EventType = eventType;
            EventId = eventId;
            NotificationTo = notificationTo;
        }

        public static EventNotification Create(IEvent eventWithMetadata, Guid toUserId)
        {
            return new EventNotification(id: Guid.NewGuid(), eventType: eventWithMetadata.Data.GetType(), eventId: eventWithMetadata.Id, notificationTo: toUserId);
        }
        
        public Guid Id { get; private set; }
        public Guid NotificationTo { get; private set; }
        public bool HasBeenViewed { get; set; }
        public bool IsArchived { get; set; }

        public Guid EventId { get; private set; }
        public Type EventType { get; private set; }
    }
}
