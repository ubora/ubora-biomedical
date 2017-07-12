using System.Collections.Generic;
using Ubora.Web._Features.Users.UserList;

namespace Ubora.Web._Features.Projects.Mentors
{
    public class MentorsViewModel
    {
        public IEnumerable<UserListItemViewModel> UboraMentors { get; set; }
        public IEnumerable<UserListItemViewModel> ProjectMentors { get; set; }
    }
}