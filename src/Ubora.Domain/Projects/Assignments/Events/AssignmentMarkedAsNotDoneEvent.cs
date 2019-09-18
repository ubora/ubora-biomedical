using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Assignments.Events
{
    public class AssignmentMarkedAsNotDoneEvent : ProjectEvent, IAssignmentEvent
    {
        public AssignmentMarkedAsNotDoneEvent(UserInfo initiatedBy, Guid projectId, Guid id) : base(initiatedBy, projectId)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

        public override string GetDescription()
        {
            return $"marked assignment \"{StringTokens.Task(Id)}\" as not done";
        }
    }
}
