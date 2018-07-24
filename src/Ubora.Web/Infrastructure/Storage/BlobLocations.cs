using System;
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

        public static BlobLocation GetRepositoryFileBlobLocation(Guid projectId, string fileName)
        {
            var containerName = BlobLocation.ContainerNames.Projects;
            var blobPath = $"{projectId}/repository/{Guid.NewGuid()}/{fileName}";

            return new BlobLocation(containerName, blobPath);
        }

        public static BlobLocation GetProjectCandidateBlobLocation(Guid projectId, Guid candidateId)
        {
            var containerName = BlobLocation.ContainerNames.Projects;
            var blobPath = $"{projectId}/candidates/{candidateId}/candidate-image/";

            return new BlobLocation(containerName, blobPath);
        }
    }
}
