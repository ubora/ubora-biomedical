using Marten;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users.Commands
{
    public class ChangeUserProfilePictureCommand : UserCommand
    {
        public BlobLocation BlobLocation { get; set; }

        internal class Handler : ICommandHandler<ChangeUserProfilePictureCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(ChangeUserProfilePictureCommand cmd)
            {
                var userProfile = _documentSession.LoadOrThrow<UserProfile>(cmd.Actor.UserId);

                userProfile.ProfilePictureBlobLocation = cmd.BlobLocation;

                _documentSession.Store(userProfile);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
