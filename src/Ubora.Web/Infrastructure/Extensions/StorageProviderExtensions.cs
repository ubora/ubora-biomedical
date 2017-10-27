using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web.Infrastructure.Extensions
{
    public static class StorageProviderExtensions
    {
        public static string GetDefaultOrBlobUrl(this ImageStorageProvider storageProvider, UserProfile userProfile)
        {
            if (userProfile.ProfilePictureBlobLocation == null)
            {
                return "/images/profileimagedefault.svg";
            }

            var blobLocation = userProfile.ProfilePictureBlobLocation;
            return storageProvider.GetUrl(blobLocation);
        }

        public static string GetDefaultOrBlobImageUrl(this ImageStorageProvider storageProvider, BlobLocation blobLocation, ImageSize imageSize)
        {
            if (blobLocation == null)
            {
                return "/images/projectlogodefault_thumbnail.png";
            }

            return storageProvider.GetUrl(blobLocation, imageSize);
        }
    }
}
