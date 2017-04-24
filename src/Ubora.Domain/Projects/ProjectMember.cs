using System;

namespace Ubora.Domain.Projects
{
    public abstract class ProjectMember
    {
        protected ProjectMember(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
