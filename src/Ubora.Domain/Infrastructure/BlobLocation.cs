namespace Ubora.Domain.Infrastructure
{
    public class BlobLocation
    {
        public string ContainerName { get; }
        public string BlobName { get; }

        public static ContainerNames ContainerNames = new ContainerNames();

        public BlobLocation(string containerName, string blobName)
        {
            ContainerName = containerName;
            BlobName = blobName;
        }
    }

    public class ContainerNames
    {
        public string Projects => "projects";
        public string Users => "users";
    }
}