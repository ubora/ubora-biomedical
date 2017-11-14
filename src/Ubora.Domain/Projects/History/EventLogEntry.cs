using System;
using Marten.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.History
{
    public class EventLogEntry : IProjectEntity
    {
        private EventLogEntry()
        { }

        public static EventLogEntry FromEvent<TEvent>(Event<TEvent> projectEvent) where TEvent : ProjectEvent
        {
            return new EventLogEntry
            {
                Id = projectEvent.Id,
                Event = projectEvent.Data,
                Timestamp = projectEvent.Data.Timestamp,
                ProjectId = projectEvent.Data.ProjectId,
                UserId = projectEvent.Data.InitiatedBy.UserId
            };
        }
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid UserId { get; private set; }
        public ProjectEvent Event { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
    }
}