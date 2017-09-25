using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users.Commands
{
    public class CreateUserProfileCommand : UserCommand
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string Biography { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }

        internal class Handler : ICommandHandler<CreateUserProfileCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(CreateUserProfileCommand cmd)
            {
                var userProfile = new UserProfile(cmd.UserId)
                {
                    Email = cmd.Email,
                    FirstName = cmd.FirstName,
                    LastName = cmd.LastName,
                    University = cmd.University,
                    Degree = cmd.Degree,
                    Field = cmd.Field,
                    Biography = cmd.Biography,
                    Skills = cmd.Skills,
                    Role = cmd.Role
                };

                _documentSession.Store(userProfile);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
