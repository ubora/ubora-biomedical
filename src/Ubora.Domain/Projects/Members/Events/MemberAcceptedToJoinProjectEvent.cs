using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Members.Events
{
    public class MemberAcceptedToJoinProjectEvent : ProjectEvent
    {
        public MemberAcceptedToJoinProjectEvent(UserInfo initiatedBy, Guid projectId, Guid userId) : base(initiatedBy, projectId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }

        public override string GetDescription() => $"accepted {StringTokens.User(UserId)} to join project.";
    }
}
