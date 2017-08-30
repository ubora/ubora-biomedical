using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Members.Events
{
    public class MemberRemovedFromProjectEvent : UboraEvent
    {
        public MemberRemovedFromProjectEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public Guid ProjectId { get; internal set; }
        public Guid UserId { get; internal set; }
        public string UserFullName { get; internal set; }

        public override string GetDescription() => $"removed {StringTokens.User(UserId)} from project.";
    }
}
