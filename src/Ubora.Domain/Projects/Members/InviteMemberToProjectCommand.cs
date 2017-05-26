using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Members
{
    public class InviteMemberToProjectCommand : UserProjectCommand
    {
        public Guid UserId { get; set; }
    }
}
