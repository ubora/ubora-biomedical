using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web.Infrastructure.Extensions
{
    public static class StorageProviderExtensions
    {
        public static string GetDefaultOrBlobUrl(this ImageStorageProvider storageProvider, UserProfile userProfile)
        {
            if (userProfile.ProfilePictureBlobName == null)
            {
                return "/images/profileimagedefault.png";
            }

            var blobLocation = BlobLocations.GetUserProfilePictureLocation(userProfile.UserId, userProfile.ProfilePictureBlobName);
            return storageProvider.GetUrl(blobLocation);
        }
    }
}
