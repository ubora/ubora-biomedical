using System;
using System.IO;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web.Infrastructure.Storage
{
    public class UboraStorageProvider : IUboraStorageProvider
    {
        private readonly IStorageProvider _storageProvider;

        public UboraStorageProvider(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public async Task SavePrivate(BlobLocation blobLocation, Stream stream)
        {
            var blobProperties = new BlobProperties
            {
                Security = BlobSecurity.Private
            };

            await _storageProvider.SaveBlobStreamAsync(blobLocation.ContainerName, blobLocation.BlobPath, stream, blobProperties);
        }

        public async Task SavePublic(BlobLocation blobLocation, Stream stream)
        {
            var blobProperties = new BlobProperties
            {
                Security = BlobSecurity.Public
            };

            await _storageProvider.SaveBlobStreamAsync(blobLocation.ContainerName, blobLocation.BlobPath, stream, blobProperties);
        }

        public string GetReadUrl(BlobLocation blobLocation, DateTime expiry)
        {
            if (blobLocation == null)
            {
                throw new ArgumentNullException(nameof(blobLocation));
            }

            var blobUrl = _storageProvider.GetBlobSasUrl(blobLocation.ContainerName, blobLocation.BlobPath, expiry);

            return blobUrl;
        }
    }
}
