using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web.Infrastructure.Extensions
{
    public static class StorageProviderExtensions
    {
        public static string GetDefaultOrBlobUrl(this ImageStorageProvider storageProvider, UserProfile userProfile)
        {
            if (userProfile.ProfilePictureBlobLocation == null)
            {
                return "/images/profileimagedefault.svg";
            }

            var blobLocation = userProfile.ProfilePictureBlobLocation;
            return storageProvider.GetUrl(blobLocation);
        }
    }
}
