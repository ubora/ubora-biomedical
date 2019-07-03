using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Notifications
{
    public class EventNotification : INotification
    {
        [JsonConstructor]
        private EventNotification(Guid id, Type eventType, Guid eventId, Guid notificationTo, DateTime createdAt)
        {
            Id = id;
            EventType = eventType;
            EventId = eventId;
            NotificationTo = notificationTo;
            CreatedAt = createdAt;
        }

        public static EventNotification Create(object @event, Guid eventId, Guid toUserId)
        {
            return new EventNotification(id: Guid.NewGuid(), eventType: @event.GetType(), eventId: eventId, notificationTo: toUserId, createdAt: DateTime.UtcNow);
        }
        
        public Guid Id { get; }
        public Guid NotificationTo { get; }
        public Guid EventId { get; }
        public Type EventType { get; }
        public DateTime CreatedAt { get; }

        public bool HasBeenViewed { get; set; }
        public bool IsArchived { get; set; }
    }
}