using System;

namespace Ubora.Domain.Projects.Members
{
    public class ProjectMentor : ProjectMember
    {
        public ProjectMentor(Guid userId) : base(userId)
        {
        }

        public override string RoleKey => "project-mentor";
    }
}