using System;
using Marten.Schema;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Users
{
    public class UserProfile : ISoftDeletable
    {
        protected UserProfile()
        {
        }

        public UserProfile(Guid userId)
        {
            UserId = userId;
        }

        [Identity]
        public Guid UserId { get; private set; }

        public string Email { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string Biography { get; internal set; }
        public Country Country { get; internal set; }
        public string Degree { get; internal set; }
        public string Field { get; internal set; }
        public string University { get; internal set; }
        public string MedicalDevice { get; internal set; }
        public string Institution { get; internal set; }
        public string Skills { get; internal set; }
        public string Role { get; internal set; }
        public BlobLocation ProfilePictureBlobLocation { get; internal set; }
        public bool IsDeleted { get; internal set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}