using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web.Infrastructure.ImageServices
{
    public class ImageStorageProvider
    {
        private readonly IStorageProvider _storageProvider;
        private readonly IUboraStorageProvider _uboraStorageProvider;

        public ImageStorageProvider(IStorageProvider storageProvider, IUboraStorageProvider uboraStorageProvider)
        {
            _storageProvider = storageProvider;
            _uboraStorageProvider = uboraStorageProvider;
        }

        protected ImageStorageProvider()
        {
        }

        // https://andrewlock.net/using-imagesharp-to-resize-images-in-asp-net-core-part-2/
        public virtual async Task SaveImageAsync(Stream stream, BlobLocation blobLocation, SizeOptions sizeOptions)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (blobLocation == null) throw new ArgumentNullException(nameof(blobLocation));
            if (sizeOptions == null) throw new ArgumentNullException(nameof(sizeOptions));

            foreach (ImageSize imageSize in sizeOptions)
            {
                var fullBlobLocation = new BlobLocation(blobLocation.ContainerName, $"{blobLocation.BlobPath}{imageSize}.jpg");

                await SaveAsync(fullBlobLocation, stream, imageSize.Width, imageSize.Height);
            }

            if (sizeOptions.IncludeOriginal)
            {
                var fullOriginalBlobLocation = new BlobLocation(blobLocation.ContainerName, $"{blobLocation.BlobPath}original.jpg");

                await SaveAsJpegAsync(fullOriginalBlobLocation, stream);
            }
        }

        public virtual async Task SaveImageAsync(Stream stream, BlobLocation blobLocation)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (blobLocation == null) throw new ArgumentNullException(nameof(blobLocation));

            await SaveStreamToBlobAsync(blobLocation, stream);
        }

        public virtual string GetUrl(BlobLocation blobLocation, ImageSize size)
        {
            if (blobLocation == null) throw new ArgumentNullException(nameof(blobLocation));
            if (size == null) throw new ArgumentNullException(nameof(size));

            var blobUrl = _storageProvider.GetBlobUrl(blobLocation.ContainerName, $"{blobLocation.BlobPath}{size}.jpg");

            return blobUrl;
        }

        public virtual string GetUrl(BlobLocation blobLocation)
        {
            if (blobLocation == null) throw new ArgumentNullException(nameof(blobLocation));

            return _storageProvider.GetBlobUrl(blobLocation.ContainerName, blobLocation.BlobPath);
        }

        public virtual async Task DeleteImagesAsync(BlobLocation blobLocation)
        {
            if (blobLocation == null) throw new ArgumentNullException(nameof(blobLocation));

            await _storageProvider.DeleteBlobAsync(blobLocation.ContainerName, blobLocation.BlobPath);
        }

        private async Task<IEnumerable<BlobDescriptor>> GetBlobs(BlobLocation blobLocation)
        {
            if (blobLocation == null) throw new ArgumentNullException(nameof(blobLocation));

            var allBlobs = await _storageProvider.ListBlobsAsync(blobLocation.ContainerName);

            return allBlobs.Where(x => x.Url.Contains(blobLocation.BlobPath));
        }

        private async Task SaveAsync(BlobLocation blobLocation, Stream stream, int width, int height)
        {
            if (blobLocation == null) throw new ArgumentNullException(nameof(blobLocation));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            stream.Seek(0, SeekOrigin.Begin);

            using (var outputStream = new MemoryStream())
            {
                using (var image = Image.Load(stream))
                {
                    image.Mutate(i => i.Resize(width, height));
                    image.SaveAsJpeg(outputStream);
                }
                outputStream.Seek(0, SeekOrigin.Begin);

                await _uboraStorageProvider.SavePublic(blobLocation, outputStream);
            }
        }

        private async Task SaveAsJpegAsync(BlobLocation blobLocation, Stream stream)
        {
            if (blobLocation == null) throw new ArgumentNullException(nameof(blobLocation));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            stream.Seek(0, SeekOrigin.Begin);

            using (var outputStream = new MemoryStream())
            {
                using (var image = Image.Load(stream))
                {
                    image.SaveAsJpeg(outputStream);
                }

                outputStream.Seek(0, SeekOrigin.Begin);

                await _uboraStorageProvider.SavePublic(blobLocation, outputStream);
            }
        }

        private async Task SaveStreamToBlobAsync(BlobLocation blobLocation, Stream stream)
        {
            if (blobLocation == null) throw new ArgumentNullException(nameof(blobLocation));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var blobProperties = new BlobProperties
            {
                Security = BlobSecurity.Public
            };

            await _storageProvider.SaveBlobStreamAsync(blobLocation.ContainerName, blobLocation.BlobPath, stream, blobProperties);
        }
    }
}
