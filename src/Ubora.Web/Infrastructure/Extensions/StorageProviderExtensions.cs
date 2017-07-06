using System.Threading.Tasks;
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
                    .GetBlobUrl($"users/{userProfile.UserId}/profile-pictures", userProfile.ProfilePictureBlobName);
            }

            return blobUrl;
        }

        public static string GetDefaultOrBlobUrl(this IStorageProvider storageProvider, Project project, int width, int height)
        {
            if (project.ProjectImageLastUpdated == null)
            {
                return $"//placehold.it/{width}x{height}";
            }

            var blobUrl = storageProvider.GetBlobUrl($"projects/{project.Id}/project-image", $"{width}x{height}.jpg");

            blobUrl = $"{blobUrl}?{project.ProjectImageLastUpdated.Value.ToString("ddMMyyyyhhss")}";

            return blobUrl;
        }

        public static async Task<bool> FileExistsAsync(this IStorageProvider storageProvider, string containerName, string blobName)
        {
            try
            {
                await storageProvider.GetBlobDescriptorAsync(containerName, blobName);

                return true;
            }
            catch (StorageException ex)
            {
                return false;
            }
        }
    }
}
