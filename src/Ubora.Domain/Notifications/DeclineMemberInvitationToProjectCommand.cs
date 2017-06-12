using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Notifications
{
    public class DeclineMemberInvitationToProjectCommand : UserCommand
    {
        public Guid InvitationId { get; set; }
    }
}
