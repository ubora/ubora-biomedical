using TwentyTwenty.Storage;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

namespace Ubora.Web.Infrastructure.Extensions
{
    public static class StorageProviderExtensions
    {
        public static string GetDefaultOrBlobUrl(this IStorageProvider storageProvider, UserProfile userProfile)
        {
            string blobUrl;
            if (userProfile.ProfilePictureBlobName == null)
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

        public static string GetDefaultOrBlobUrl(this IStorageProvider storageProvider, Project project)
        {
            if (string.IsNullOrEmpty(project.ImageBlobName))
            {
                return "//placehold.it/1500x300";
            }

            return storageProvider
                    .GetBlobUrl($"projects/{project.Id}/project-image", project.ImageBlobName)
                    .Replace("/app/wwwroot", "");
        }
    }
}
