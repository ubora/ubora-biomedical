using System;
using System.IO;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web.Infrastructure.Storage
{
    public interface IUboraStorageProvider
    {
        Task SavePrivate(BlobLocation blobLocation, Stream stream);
        Task SavePublic(BlobLocation blobLocation, Stream stream);
        string GetReadUrl(BlobLocation blobLocation, DateTime expiry);
    }
}
