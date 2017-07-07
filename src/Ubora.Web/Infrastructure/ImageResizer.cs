using ImageSharp;
using SixLabors.Primitives;
using System.IO;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web.Infrastructure
{
    public class ImageResizer
    {
        private readonly IStorageProvider _storageProvider;

        public ImageResizer(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        protected ImageResizer()
        {

        }

        // https://andrewlock.net/using-imagesharp-to-resize-images-in-asp-net-core-part-2/
        public virtual async Task CreateResizedImageAndSaveAsJpegAsync(BlobLocation blobLocation, Stream stream, int width, int height)
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

                await SaveStreamToBlobAsync(blobLocation, outputStream);
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

        public virtual async Task SaveAsJpegAsync(BlobLocation blobLocation, Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using (var outputStream = new MemoryStream())
            {
                using (var image = Image.Load(stream))
                {
                    image.SaveAsJpeg(outputStream);
                }

                outputStream.Seek(0, SeekOrigin.Begin);

                await SaveStreamToBlobAsync(blobLocation, outputStream);
            }
        }

        private async Task SaveStreamToBlobAsync(BlobLocation blobLocation, Stream stream)
        {
            var blobProperties = new BlobProperties
            {
                Security = BlobSecurity.Public
            };

            var fileExists = await _storageProvider.FileExistsAsync(blobLocation.ContainerName, blobLocation.BlobName);

            if (fileExists)
            {
                await _storageProvider.DeleteBlobAsync(blobLocation.ContainerName, blobLocation.BlobName);
            }

            await _storageProvider.SaveBlobStreamAsync(blobLocation.ContainerName, blobLocation.BlobName, stream, blobProperties);
        }
    }
}
