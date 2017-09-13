using System;
using System.Text.RegularExpressions;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web.Infrastructure.Storage
{
    public static class BlobLocations
    {
        public static BlobLocation GetProjectImageBlobLocation(Guid projectId)
        {
            var containerName = BlobLocation.ContainerNames.Projects;
            var blobPath = $"{projectId}/project-image/";

            return new BlobLocation(containerName, blobPath);
        }

        public static BlobLocation GetUserProfilePictureLocation(Guid userId, string profilePictureBlobName)
        {
            var containerName = BlobLocation.ContainerNames.Users;
            var blobPath = $"{userId}/profile-pictures/{profilePictureBlobName}";

            return new BlobLocation(containerName, blobPath);
        }

        public static BlobLocation GetRepositoryFileBlobLocation(Guid projectId, string folderName, string fileName)
        {
            var folderNameToLower = folderName?.ToLower();
            var modifiedFolderName = Regex.Replace(folderName, @"\s+", "-");

            var containerName = BlobLocation.ContainerNames.Projects;
            var blobPath = $"{projectId}/repository/{folderName}/{Guid.NewGuid()}/{fileName}";

            return new BlobLocation(containerName, blobPath, folderName);
        }
    }
}
