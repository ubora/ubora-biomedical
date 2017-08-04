using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Local;

namespace Ubora.Web.Infrastructure
{
    public class FixedLocalStorageProvider : IStorageProvider
    {
        private readonly string _basePath;
        private readonly LocalStorageProvider _localStorageProvider;

        public FixedLocalStorageProvider(string basePath, LocalStorageProvider localStorageProvider)
        {
            _basePath = basePath;
            _localStorageProvider = localStorageProvider;
        }

        /// <summary>
        /// Problem: LocalStorageProvider didn't create directory from path which was specified in 'blobName'.
        /// Solution: Combine full path and ensure it exists.
        /// </summary>
        private void EnsureDirectoryExists(string containerName, string blobName)
        {
            var combined = Path.Combine(_basePath, containerName, blobName);
            var directoryPath = Path.GetDirectoryName(combined);
            Directory.CreateDirectory(directoryPath);
        }

        public Task SaveBlobStreamAsync(string containerName, string blobName, Stream source, BlobProperties properties = null, bool closeStream = true)
        {
            EnsureDirectoryExists(containerName, blobName);
            return _localStorageProvider.SaveBlobStreamAsync(containerName, blobName, source, properties, closeStream);
        }

        public Task<Stream> GetBlobStreamAsync(string containerName, string blobName)
        {
            return _localStorageProvider.GetBlobStreamAsync(containerName, blobName);
        }

        public Task<BlobDescriptor> GetBlobDescriptorAsync(string containerName, string blobName)
        {
            return _localStorageProvider.GetBlobDescriptorAsync(containerName, blobName);
        }

        public Task<IList<BlobDescriptor>> ListBlobsAsync(string containerName)
        {
            return _localStorageProvider.ListBlobsAsync(containerName);
        }

        public Task DeleteBlobAsync(string containerName, string blobName)
        {
            return _localStorageProvider.DeleteBlobAsync(containerName, blobName);
        }

        public Task DeleteContainerAsync(string containerName)
        {
            return _localStorageProvider.DeleteContainerAsync(containerName);
        }

        public Task UpdateBlobPropertiesAsync(string containerName, string blobName, BlobProperties properties)
        {
            return _localStorageProvider.UpdateBlobPropertiesAsync(containerName, blobName, properties);
        }

        public string GetBlobUrl(string containerName, string blobName)
        {
            var localPath = _localStorageProvider.GetBlobUrl(containerName, blobName);

            // 1. Make local absolute path into relative path because browsers don't open files from local absolute path without permission.
            // 2. Fix Docker relative path by replacing 'app/wwwroot' part.
            var indexOfRelativePath = localPath.IndexOf("wwwroot", StringComparison.OrdinalIgnoreCase) + "wwwroot".Length;
            var localBlobUrl = localPath.Substring(indexOfRelativePath);

            return localBlobUrl;
        }

        public string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry, bool isDownload = false, string fileName = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read)
        {
            return _localStorageProvider.GetBlobSasUrl(containerName, blobName, expiry, isDownload, fileName, contentType, access);
        }
    }
}