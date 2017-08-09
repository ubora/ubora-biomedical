using System;
using Marten.Schema;
using Ubora.Domain.Infrastructure;

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

        [Identity]
        public Guid UserId { get; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public Country Country { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string University { get; set; }
        public string MedicalDevice { get; set; }
        public string Institution { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }
        public BlobLocation ProfilePictureBlobLocation { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}