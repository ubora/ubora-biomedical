using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Tasks.Events
{
    public class TaskAddedEvent : ProjectEvent, ITaskEvent
    {
        public TaskAddedEvent(UserInfo initiatedBy, Guid projectId, Guid id, string title, string description, IEnumerable<Guid> assigneeIds) : base(initiatedBy, projectId)
        {
            Id = id;
            Title = title;
            Description = description;
            AssigneeIds = assigneeIds;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<Guid> AssigneeIds { get; private set; }

        public override string GetDescription()
        {
            return $"added task \"{StringTokens.Task(Id)}\"";
        }
    }
}