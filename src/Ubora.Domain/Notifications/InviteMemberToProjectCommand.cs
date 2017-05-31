using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Notifications
{
    public class InviteMemberToProjectCommand : UserProjectCommand
    {
        public Guid InvitedMemberId { get; set; }
    }
}
