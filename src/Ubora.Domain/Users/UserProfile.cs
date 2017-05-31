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

        public void SetNewGuidBlobName()
        {
            BlobName = Guid.NewGuid().ToString();
        }

        [Identity]
        public Guid UserId { get; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string Biography { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }
        public string BlobName { get; set; } = "Default";

        public string FullName => $"{FirstName} {LastName}";
    }
}