using System;

namespace Ubora.Domain.Projects.Members
{
    public class ProjectMentor : UserProfile
    {
        public ProjectMentor(Guid userId) : base(userId)
        {
        }

        public override string RoleKey => "project-mentor";
    }
}