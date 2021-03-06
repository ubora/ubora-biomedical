﻿using System;

namespace Ubora.Web._Features.Users.UserList.Models
{
    public class UserListItemViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string University { get; set; }
        public string ProfilePictureLink { get; set; }
        public string Role { get; set; }
        public bool IsInvited { get; set; }
    }
}