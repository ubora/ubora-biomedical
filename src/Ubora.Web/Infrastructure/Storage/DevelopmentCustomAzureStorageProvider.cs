using System;
using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Azure;

namespace Ubora.Web.Infrastructure.Storage
{
    public class CustomDevelopmentAzureStorageProvider : CustomAzureStorageProvider
    {
        private readonly string _linkLocalIpAddress;
        
        public CustomDevelopmentAzureStorageProvider(AzureProviderOptions options, AzureStorageProvider azureStorageProvider, string linkLocalIpAddress) : base(options, azureStorageProvider)
        {
            _linkLocalIpAddress = linkLocalIpAddress;
        }

        public override string GetBlobUrl(string containerName, string blobName)
        {
            var blobUrl = base.GetBlobUrl(containerName, blobName);
            
            var isLinked = !String.IsNullOrEmpty(_linkLocalIpAddress);
            if (isLinked)
            {
                return blobUrl.Replace("http://127.0.0.1:32500/devstoreaccount1", $"http://{_linkLocalIpAddress}:32500/devstoreaccount1"); 
            }
            
            return blobUrl.Replace("http://azurite:10000/devstoreaccount1", "http://localhost:32500/devstoreaccount1"); 
        }

        // For development because azurite does not support this yet!
        public override string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry, bool isDownload = false, string fileName = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read)
        {
            return GetBlobUrl(containerName, blobName);
        }
    }
}
