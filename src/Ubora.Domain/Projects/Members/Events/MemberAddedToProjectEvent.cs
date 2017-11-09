using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Members.Events
{
    public class MemberAddedToProjectEvent : ProjectEvent
    {
        public MemberAddedToProjectEvent(UserInfo initiatedBy, Guid projectId, Guid userId) : base(initiatedBy, projectId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }

        public override string GetDescription() => $"{StringTokens.User(UserId)} was added to project.";
    }
}