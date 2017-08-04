using System;
using Ubora.Domain.Projects.Members;

namespace Ubora.Domain.Projects
{
    public class HasLeader : HasMember<ProjectLeader>
    {
        public HasLeader(Guid userId) : base(userId)
        {
        }
    }
}
