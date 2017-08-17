using ImageSharp;
using SixLabors.Primitives;
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
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (blobLocation == null)
            {
                throw new ArgumentNullException(nameof(blobLocation));
            }

            if (sizeOptions == null)
            {
                throw new ArgumentNullException(nameof(sizeOptions));
            }

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
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (blobLocation == null)
            {
                throw new ArgumentNullException(nameof(blobLocation));
            }

            await _uboraStorageProvider.SavePublic(blobLocation, stream);
        }

        public virtual string GetUrl(BlobLocation blobLocation, ImageSize size)
        {
            if (blobLocation == null)
            {
                throw new ArgumentNullException(nameof(blobLocation));
            }

            if (size == null)
            {
                throw new ArgumentNullException(nameof(size));
            }

            var blobUrl = _storageProvider.GetBlobUrl(blobLocation.ContainerName, $"{blobLocation.BlobPath}{size}.jpg");

            return blobUrl;
        }

        public virtual string GetUrl(BlobLocation blobLocation)
        {
            if (blobLocation == null)
            {
                throw new ArgumentNullException(nameof(blobLocation));
            }

            return _storageProvider.GetBlobUrl(blobLocation.ContainerName, blobLocation.BlobPath);
        }

        public virtual async Task DeleteImagesAsync(BlobLocation blobLocation)
        {
            if (blobLocation == null)
            {
                throw new ArgumentNullException(nameof(blobLocation));
            }

            await _storageProvider.DeleteBlobAsync(blobLocation.ContainerName, blobLocation.BlobPath);
        }

        private async Task<IEnumerable<BlobDescriptor>> GetBlobs(BlobLocation blobLocation)
        {
            var allBlobs = await _storageProvider.ListBlobsAsync(blobLocation.ContainerName);

            return allBlobs.Where(x => x.Url.Contains(blobLocation.BlobPath));
        }

        private async Task SaveAsync(BlobLocation blobLocation, Stream stream, int width, int height)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using (var outputStream = new MemoryStream())
            {
                using (var image = Image.Load(stream))
                {
                    var cropRectangle = GetRectangle(width, height, image.Width, image.Height);

                    image.Crop(cropRectangle)
                         .Resize(width, height)
                         .SaveAsJpeg(outputStream);
                }
                outputStream.Seek(0, SeekOrigin.Begin);

                await _uboraStorageProvider.SavePublic(blobLocation, outputStream);
            }
        }

        private async Task SaveAsJpegAsync(BlobLocation blobLocation, Stream stream)
        {
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

        private Rectangle GetRectangle(int width, int height, int imageWidth, int imageHeight)
        {
            var ratio = width / height;
            var imageRatio = imageWidth / imageHeight;

            if (imageRatio < ratio)
            {
                var newHeight = (imageWidth * height) / width;
                var yPosition = (imageHeight / 2) - (newHeight / 2);

                return new Rectangle(0, yPosition, imageWidth, newHeight);
            }
            else if (imageRatio > ratio)
            {

                var newWidth = (imageHeight * width) / height;
                var xPosition = (imageWidth / 2) - (newWidth / 2);

                return new Rectangle(xPosition, 0, newWidth, imageHeight);
            }

            return new Rectangle(0, 0, width, height);
        }
    }
}
