using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Assignments.Events
{
    public class AssignmentEditedEvent : ProjectEvent, IAssignmentEvent
    {
        public AssignmentEditedEvent(UserInfo initiatedBy, Guid projectId, string title, string description, Guid id, IEnumerable<Guid> assigneeIds) 
            : base(initiatedBy, projectId)
        {
            Title = title;
            Description = description;
            Id = id;
            AssigneeIds = assigneeIds?.ToArray() ?? Array.Empty<Guid>();
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid Id { get; private set; }
        public IEnumerable<Guid> AssigneeIds { get; private set; }

        public override string GetDescription()
        {
            return $"edited assignment \"{StringTokens.Task(Id)}\"";
        }
    }
}