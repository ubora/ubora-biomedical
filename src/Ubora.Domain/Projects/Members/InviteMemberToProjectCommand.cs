using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Members
{
    public class InviteMemberToProjectCommand : UserCommand
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }
}
