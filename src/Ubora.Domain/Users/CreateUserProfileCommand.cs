using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users
{
    public class CreateUserProfileCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string Biography { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }
    }
}
