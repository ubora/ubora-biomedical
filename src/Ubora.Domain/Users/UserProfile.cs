﻿using System;
using Marten.Schema;

namespace Ubora.Domain.Users
{
    public class UserProfile
    {
        [Identity]
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Country { get; set; }
        public string Biography { get; set; }
        public string Skills { get; set; }
    }
}