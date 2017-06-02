using System;
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
            var blobProperties = new BlobProperties()
            {
                Security = BlobSecurity.Public,
                ContentType = cmd.ContentType,
                ContentDisposition = cmd.ContentDisposition 
            };

            var userProfile = _documentSession.Load<UserProfile>(cmd.UserId);

            var blobName = Guid.NewGuid() + cmd.FileName;
            //userProfile.SetNewGuidBlobName();
            userProfile.BlobName = blobName;

            _documentSession.Store(userProfile);
            _documentSession.SaveChanges();

            _storageProvider.SaveBlobStreamAsync("profilePictures", blobName, cmd.Stream, blobProperties).Wait();

            return new CommandResult();
        }
    }
}
