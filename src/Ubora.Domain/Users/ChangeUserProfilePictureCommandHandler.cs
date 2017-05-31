using Marten;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users
{
    public class ChangeUserProfilePictureCommandHandler : ICommandHandler<ChangeUserProfilePictureCommand>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IStorageProvider _storageProvider;

        public ChangeUserProfilePictureCommandHandler(IDocumentSession documentSession, IStorageProvider storageProvider)
        {
            _documentSession = documentSession;
            _storageProvider = storageProvider;
        }

        public ICommandResult Handle(ChangeUserProfilePictureCommand cmd)
        {
            var userProfile = _documentSession.Load<UserProfile>(cmd.UserId);

            userProfile.SetNewGuidBlobName();

            _documentSession.Store(userProfile);
            _documentSession.SaveChanges();

            _storageProvider.SaveBlobStreamAsync("profilePictures", userProfile.BlobName, cmd.FileStream).Wait();

            return new CommandResult();
        }
    }
}
