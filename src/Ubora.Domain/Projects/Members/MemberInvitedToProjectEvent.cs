using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Members
{
    internal class MemberInvitedToProjectEvent:UboraEvent
    {
        public MemberInvitedToProjectEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }

        public override string GetDescription() => $"{UserFullName} was invited to project.";
    }
}