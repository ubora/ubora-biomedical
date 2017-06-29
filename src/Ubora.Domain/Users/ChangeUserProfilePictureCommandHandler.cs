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
            var blobProperties = new BlobProperties
            {
                Security = BlobSecurity.Public
            };

            var userProfile = _documentSession.Load<UserProfile>(cmd.UserId);

            if (userProfile.ProfilePictureBlobName != null)
            {
                _storageProvider.DeleteBlobAsync("users", $"{userProfile.UserId}/profile-pictures/{userProfile.ProfilePictureBlobName}");
            }

            userProfile.ProfilePictureBlobName = cmd.FileName;

            _storageProvider.SaveBlobStreamAsync("users", $"{userProfile.UserId}/profile-pictures/{userProfile.ProfilePictureBlobName}", cmd.Stream, blobProperties)
                .Wait();

            _documentSession.Store(userProfile);
            _documentSession.SaveChanges();


            return new CommandResult();
        }
    }
}
