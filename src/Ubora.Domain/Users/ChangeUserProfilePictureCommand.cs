using System;
using System.IO;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users
{
    public class ChangeUserProfilePictureCommand : UserCommand
    {
        public Guid UserId { get; set; }
        public Stream Stream { get; set; }
        public string FileName { get; set; }
    }
}
