using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Tasks
{
    public class TaskEditedEvent : UboraEvent, ITaskEvent
    {
        public TaskEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }

        public override string GetDescription()
        {
            return $"edited task \"{StringTokens.Task(Id)}\"";
        }
    }
}