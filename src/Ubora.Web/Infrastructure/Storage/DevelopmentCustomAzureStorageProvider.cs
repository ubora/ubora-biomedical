using System;
using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Azure;

namespace Ubora.Web.Infrastructure.Storage
{
    public class CustomDevelopmentAzureStorageProvider : CustomAzureStorageProvider
    {
        public CustomDevelopmentAzureStorageProvider(AzureProviderOptions options, AzureStorageProvider azureStorageProvider) : base(options, azureStorageProvider)
        {
        }

        public override string GetBlobUrl(string containerName, string blobName)
        {
            var blobUrl = base.GetBlobUrl(containerName, blobName);

            return blobUrl.Replace("http://azurite:10000/devstoreaccount1", "http://localhost:32500/devstoreaccount1");
        }

        // For development because azurite does not support this yet! Mardo brain not function jet!
        public override string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry, bool isDownload = false, string fileName = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read)
        {
            return GetBlobUrl(containerName, blobName);
        }
    }
}
