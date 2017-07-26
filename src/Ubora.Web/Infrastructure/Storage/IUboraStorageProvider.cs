using System;
using System.IO;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web.Infrastructure.Storage
{
    public interface IUboraStorageProvider
    {
        Task SavePrivateStreamToBlobAsync(BlobLocation blobLocation, Stream stream);
        Task SavePublicStreamToBlobAsync(BlobLocation blobLocation, Stream stream);
        string GetBlobSasUrl(BlobLocation blobLocation, DateTime expiry);
    }
}
