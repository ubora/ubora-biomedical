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
        public bool IsLeader => this is ProjectLeader;
        public bool IsMentor => this is ProjectMentor;
    }
}
