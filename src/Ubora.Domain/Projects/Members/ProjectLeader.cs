using System;

namespace Ubora.Domain.Projects.Members
{
    public class ProjectLeader : UserProfile
    {
        public ProjectLeader(Guid userId) : base(userId)
        {
        }

        public override string RoleKey => "project-leader";
    }
}