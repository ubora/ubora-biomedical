using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Members
{
    public class ProjectMemberListViewModel
    {
        public Guid Id { get; set; }
        public IEnumerable<Item> Members { get; set; }
        public bool CanRemoveProjectMembers { get; set; }

        public class Item
        {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
        }
    }
}