using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users
{
    public class EditUserProfileCommand : UserCommand
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string CountryCode { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string University { get; set; }
        public string MedicalDevice { get; set; }
        public string Institution { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }
        public bool IsFirstTimeEditedProfile { get; set; }
    }
}