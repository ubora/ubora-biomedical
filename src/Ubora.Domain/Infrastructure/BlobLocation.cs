namespace Ubora.Domain.Infrastructure
{
    public class BlobLocation
    {
        public string ContainerName { get; }
        public string BlobName { get; }

        public BlobLocation(string containerName, string blobName)
        {
            ContainerName = containerName;
            BlobName = blobName;
        }

        public static class ContainerNames
        {
            public const string Projects = "projects";
            public const string Users = "users";
        }
    }
}