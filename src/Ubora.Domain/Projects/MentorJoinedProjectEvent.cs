using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    public class MentorJoinedProjectEvent : UboraEvent
    {
        public MentorJoinedProjectEvent(Guid projectId, Guid userId, string userFullName, UserInfo initiatedBy) : base(initiatedBy)
        {
            ProjectId = projectId;
            UserId = userId;
        }

        public Guid ProjectId { get; private set; }
        public Guid UserId { get; private set; }

        public override string GetDescription() => $"{InitiatedBy.Name} joined as mentor.";
    }
}
