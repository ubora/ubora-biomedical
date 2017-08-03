using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Members
{
    public class ProjectMemberListViewModel
    {
        public Guid Id { get; set; }
        public IEnumerable<Item> Members { get; set; }
        public bool CanRemoveProjectMembers { get; set; }
        public bool IsProjectMember { get; set; }
        public bool IsProjectLeader { get; set; }

        public class Item
        {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
            public bool IsProjectLeader { get; set; }
            public bool IsCurrentUser { get; set; }
            public bool IsProjectMentor { get; set; }
        }
    }
}
