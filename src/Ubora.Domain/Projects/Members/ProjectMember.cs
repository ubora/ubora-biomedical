using System;

namespace Ubora.Domain.Projects.Members
{
    public class ProjectMember
    {
        public ProjectMember(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
