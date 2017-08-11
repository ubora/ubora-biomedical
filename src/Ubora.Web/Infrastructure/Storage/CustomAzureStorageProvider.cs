using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Azure;

namespace Ubora.Web.Infrastructure.Storage
{
    public class CustomAzureStorageProvider : IStorageProvider
    {
        private readonly AzureProviderOptions _options;
        private readonly AzureStorageProvider _azureStorageProvider;
        private readonly CloudBlobClient _blobClient;
        private readonly BlobRequestOptions _requestOptions;
        private readonly OperationContext _context;

        public CustomAzureStorageProvider(
            AzureProviderOptions options,
            AzureStorageProvider azureStorageProvider)
        {
            _options = options;
            _azureStorageProvider = azureStorageProvider;
            _blobClient = CloudStorageAccount
                .Parse(options.ConnectionString)
                .CreateCloudBlobClient();
            _requestOptions = new BlobRequestOptions();
            _context = new OperationContext();
        }

        // Based on the TwentyTwenty Storage Azure Provider
        // https://github.com/2020IP/TwentyTwenty.Storage/blob/master/src/TwentyTwenty.Storage.Azure/AzureStorageProvider.cs
        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            var container = _blobClient.GetContainerReference(containerName);
            CancellationToken cancellationToken = new CancellationToken();

            try
            {
                var blobItems = await GetBlobs(container, blobName, cancellationToken);

                foreach (var blobItem in blobItems)
                {
                    await blobItem.DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, _requestOptions, _context);
                }
            }
            catch (Exception e)
            {
                if (e.IsAzureStorageException())
                {
                    throw e.Convert();
                }
                throw;
            }
        }

        private async Task<IEnumerable<CloudBlockBlob>> GetBlobs(CloudBlobContainer container, string prefix, CancellationToken cancellationToken)
        {
            var listBlobsSegmented = await container.ListBlobsSegmentedAsync(
                prefix: prefix,
                useFlatBlobListing: true,
                blobListingDetails: BlobListingDetails.None,
                maxResults: null,
                currentToken: null,
                options: new BlobRequestOptions(),
                operationContext: _context,
                cancellationToken: cancellationToken);

            return listBlobsSegmented.Results.Select(x => (CloudBlockBlob)x);
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            await _azureStorageProvider.DeleteContainerAsync(containerName);
        }

        public async Task<BlobDescriptor> GetBlobDescriptorAsync(string containerName, string blobName)
        {
            return await _azureStorageProvider.GetBlobDescriptorAsync(containerName, blobName);
        }

        public string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry, bool isDownload = false, string fileName = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read)
        {
            return _azureStorageProvider.GetBlobSasUrl(containerName, blobName, expiry, isDownload, fileName, contentType, access);
        }

        public async Task<Stream> GetBlobStreamAsync(string containerName, string blobName)
        {
            return await _azureStorageProvider.GetBlobStreamAsync(containerName, blobName);
        }

        public virtual string GetBlobUrl(string containerName, string blobName)
        {
            return _azureStorageProvider.GetBlobUrl(containerName, blobName);
        }

        public async Task<IList<BlobDescriptor>> ListBlobsAsync(string containerName)
        {
            return await _azureStorageProvider.ListBlobsAsync(containerName);
        }

        public async Task SaveBlobStreamAsync(string containerName, string blobName, Stream source, TwentyTwenty.Storage.BlobProperties properties = null, bool closeStream = true)
        {
            await _azureStorageProvider.SaveBlobStreamAsync(containerName, blobName, source, properties, closeStream);
        }

        public async Task UpdateBlobPropertiesAsync(string containerName, string blobName, TwentyTwenty.Storage.BlobProperties properties)
        {
            await _azureStorageProvider.UpdateBlobPropertiesAsync(containerName, blobName, properties);
        }
    }
}
