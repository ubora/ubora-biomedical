using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users.Commands
{
    public class DeleteUserCommand : UserCommand
    {
        public Guid UserId { get; set; }

        public class Handler : ICommandHandler<DeleteUserCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(DeleteUserCommand cmd)
            {
                var userProfile = _documentSession.LoadOrThrow<UserProfile>(cmd.UserId);
                userProfile.IsDeleted = true;

                _documentSession.Store(userProfile);
                _documentSession.Delete<UserProfile>(userProfile.UserId);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
