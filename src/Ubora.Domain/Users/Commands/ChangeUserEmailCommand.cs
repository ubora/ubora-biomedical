using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users.Commands
{
    public class ChangeUserEmailCommand : UserCommand
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }

        internal class Handler : ICommandHandler<ChangeUserEmailCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(ChangeUserEmailCommand cmd)
            {
                var userProfile = _documentSession.LoadOrThrow<UserProfile>(cmd.UserId);

                userProfile.Email = cmd.Email;

                _documentSession.Store(userProfile);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
