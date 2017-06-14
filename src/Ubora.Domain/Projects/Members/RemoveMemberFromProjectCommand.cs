using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Members
{
    public class RemoveMemberFromProjectCommand : UserProjectCommand
    {
        public Guid UserId { get; set; }
    }
}
