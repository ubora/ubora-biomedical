using System;

namespace Ubora.Domain.Projects
{
    public class ProjectLeader : ProjectMember
    {
        public ProjectLeader(Guid userId) : base(userId)
        {
        }
    }
}