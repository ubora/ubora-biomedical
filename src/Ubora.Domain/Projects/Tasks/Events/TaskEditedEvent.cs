using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Tasks.Events
{
    public class TaskEditedEvent : ProjectEvent, ITaskEvent
    {
        public TaskEditedEvent(UserInfo initiatedBy, Guid projectId, string title, string description, Guid id) : base(initiatedBy, projectId)
        {
            Title = title;
            Description = description;
            Id = id;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }

        public override string GetDescription()
        {
            return $"edited task \"{StringTokens.Task(Id)}\"";
        }
    }
}