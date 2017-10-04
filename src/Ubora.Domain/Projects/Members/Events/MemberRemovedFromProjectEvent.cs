using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Members.Events
{
    public class MemberRemovedFromProjectEvent : ProjectEvent
    {
        public MemberRemovedFromProjectEvent(UserInfo initiatedBy, Guid projectId, Guid userId) : base(initiatedBy, projectId)
        {
            UserId = userId;
        }

        public Guid UserId { get; internal set; }

        public override string GetDescription() => $"removed {StringTokens.User(UserId)} from project.";
    }
}
