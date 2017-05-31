using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Notifications
{
    public class UpdateMemberInvitationToProjectAsViewedCommand : UserCommand
    {
        public Guid InvitationId { get; set; }
    }
}
