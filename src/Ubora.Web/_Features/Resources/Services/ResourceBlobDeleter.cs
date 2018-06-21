﻿using System;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Resources;

namespace Ubora.Web._Features.Resources.Services
{
    public class ResourceBlobDeleter : IResourceBlobDeleter
    {
        private IStorageProvider _storageProvider;

        public ResourceBlobDeleter(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public async Task DeleteBlobContainerOfResourcePage(ResourcePage resourcePage)
        {
            await _storageProvider.DeleteContainerAsync(resourcePage.GetBlobContainerName());
        }

        public async Task DeleteBlobOfResourceFile(ResourceFile resourceFile)
        {
            await _storageProvider.DeleteBlobAsync(
                containerName: resourceFile.BlobLocation.ContainerName, 
                blobName: resourceFile.BlobLocation.BlobPath);
        }
    }
}