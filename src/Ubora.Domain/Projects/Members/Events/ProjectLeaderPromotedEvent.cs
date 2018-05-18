using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Members.Events
{
    public class ProjectLeaderPromotedEvent : ProjectEvent
    {
        public ProjectLeaderPromotedEvent(UserInfo initiatedBy, Guid projectId, Guid userId) : base(initiatedBy, projectId)
        {
            UserId = userId;
        }

        public Guid UserId { get; internal set; }

        public override string GetDescription() => $"Promoted to {StringTokens.User(UserId)} project leader.";
    }
}
