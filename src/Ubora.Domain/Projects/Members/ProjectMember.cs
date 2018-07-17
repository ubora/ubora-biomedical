using Newtonsoft.Json;
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

        // Override this!
        public virtual string RoleKey => "project-member";

        [JsonIgnore]
        public bool IsLeader => this is ProjectLeader;

        [JsonIgnore]
        public bool IsMentor => this is ProjectMentor;
    }
}
