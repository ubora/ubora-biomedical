using System;

namespace Ubora.Domain.Infrastructure
{
    public class BlobLocation
    {
        public string ContainerName { get; private set; }
        public string BlobPath { get; private set; }
        public string FolderName { get; private set; }

        public BlobLocation(string containerName, string blobPath, string folderName = null)
        {
            ContainerName = containerName;
            BlobPath = blobPath;
            FolderName = folderName;
        }

        public static class ContainerNames
        {
            public const string Projects = "projects";
            public const string Users = "users";
        }
    }
}