using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Assignments.Events
{
    public class AssignmentAddedEvent : ProjectEvent, IAssignmentEvent
    {
        public AssignmentAddedEvent(UserInfo initiatedBy, Guid projectId, Guid id, string title, string description, IEnumerable<Guid> assigneeIds) 
            : base(initiatedBy, projectId)
        {
            Id = id;
            Title = title;
            Description = description;
            AssigneeIds = assigneeIds?.ToArray() ?? Array.Empty<Guid>();
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<Guid> AssigneeIds { get; private set; }

        public override string GetDescription()
        {
            return $"added assignment \"{StringTokens.Task(Id)}\"";
        }
    }
}