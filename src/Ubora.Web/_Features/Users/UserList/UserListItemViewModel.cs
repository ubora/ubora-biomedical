using System;

namespace Ubora.Web._Features.Users.UserList
{
    public class UserListItemViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePictureLink { get; set; }
    }
}