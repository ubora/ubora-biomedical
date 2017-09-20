using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Members.Events
{
    internal class MemberAcceptedToJoinProjectEvent : UboraEvent
    {
        public MemberAcceptedToJoinProjectEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public Guid ProjectId { get; internal set; }
        public Guid UserId { get; internal set; }
        public string UserFullName { get; internal set; }

        public override string GetDescription() => $"accepted {StringTokens.User(UserId)} to join project.";
    }
}
