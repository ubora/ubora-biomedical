using ImageSharp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using SixLabors.Primitives;
using System;
using System.IO;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web.Infrastructure
{
    public class ImageResizer
    {
        private readonly IStorageProvider _storageProvider;
        private readonly IFileProvider _fileProvider;

        public ImageResizer(
            IStorageProvider storageProvider,
            IHostingEnvironment env)
        {
            _storageProvider = storageProvider;
            _fileProvider = env.WebRootFileProvider;
        }

        // https://andrewlock.net/using-imagesharp-to-resize-images-in-asp-net-core-part-2/
        public async Task CreateResizedImageAndSaveAsJpegAsync(string containerName, string blobName, Stream stream, int width, int height)
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

                await SaveStreamToBlobAsync(containerName, blobName, outputStream);
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

            var newWidth = (imageHeight * width) / height;
            var xPosition = (imageWidth / 2) - (newWidth / 2);

            return new Rectangle(xPosition, 0, newWidth, imageHeight);
        }

        public async Task SaveAsJpegAsync(string containerName, string blobName, Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using (var outputStream = new MemoryStream())
            {
                using (var image = Image.Load(stream))
                {
                    image.SaveAsJpeg(outputStream);
                }

                outputStream.Seek(0, SeekOrigin.Begin);

                await SaveStreamToBlobAsync(containerName, blobName, outputStream);
            }
        }

        private async Task SaveStreamToBlobAsync(string containerName, string blobName, Stream stream)
        {
            var blobProperties = new BlobProperties
            {
                Security = BlobSecurity.Public
            };

            var fileExists = await _storageProvider.FileExistsAsync(containerName, blobName);

            if (fileExists)
            {
                await _storageProvider.DeleteBlobAsync(containerName, blobName);
            }

            await _storageProvider.SaveBlobStreamAsync(containerName, blobName, stream, blobProperties);
        }
    }
}
