using TwentyTwenty.Storage;
using Ubora.Domain.Users;

namespace Ubora.Web.Infrastructure.Extensions
{
    public static class StorageProviderExtensions
    {
        public static string GetDefaultOrBlobUrl(this IStorageProvider storageProvider, UserProfile userProfile)
        {
            string blobUrl;
            if (userProfile.ProfilePictureBlobName == "Default")
            {
                blobUrl = "/images/profileimagedefault.png";
            }
            else
            {
                blobUrl = storageProvider
                    .GetBlobUrl($"users/{userProfile.UserId}/profile-pictures", userProfile.ProfilePictureBlobName)
                    .Replace("/app/wwwroot", "");
            }

            return blobUrl;
        }
    }
}
