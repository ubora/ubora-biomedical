using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Members
{
    public class ProjectMentorAssignedEvent : UboraEvent
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }

        public ProjectMentorAssignedEvent(UserInfo initiatedBy, Guid userId, Guid projectId) : base(initiatedBy)
        {
            UserId = userId;
            ProjectId = projectId;
        }

        public override string GetDescription() => "Assigned project mentor.";
    }
}
