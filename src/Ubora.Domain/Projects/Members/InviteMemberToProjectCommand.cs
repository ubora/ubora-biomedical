using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Members
{
    public class InviteMemberToProjectCommand : ICommand
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
