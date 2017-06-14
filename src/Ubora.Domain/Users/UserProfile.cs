using System;
using Marten.Schema;

namespace Ubora.Domain.Users
{
    public class UserProfile
    {
        protected UserProfile()
        {
        }

        public UserProfile(Guid userId)
        {
            UserId = userId;
        }

        public void SetProfilePictureBlobName(string fileName)
        {
            ProfilePictureBlobName = Guid.NewGuid() + fileName;
        }

        [Identity]
        public Guid UserId { get; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string Biography { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }
        public string ProfilePictureBlobName { get; private set; } = "Default";

        public string FullName => $"{FirstName} {LastName}";
    }
}