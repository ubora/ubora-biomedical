using Marten;
using System;
using System.IO;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Users
{
    public class ChangeUserProfilePictureCommand : UserCommand
    {
        public BlobLocation BlobLocation { get; set; }

        internal class Handler : ICommandHandler<ChangeUserProfilePictureCommand>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IStorageProvider _storageProvider;

            public Handler(IDocumentSession documentSession, IStorageProvider storageProvider)
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

                var userProfile = _documentSession.Load<UserProfile>(cmd.Actor.UserId);
                if (userProfile == null) throw new InvalidOperationException();

                userProfile.ProfilePictureBlobLocation = cmd.BlobLocation;

                _documentSession.Store(userProfile);
                _documentSession.SaveChanges();


                return new CommandResult();
            }
        }
    }
}
