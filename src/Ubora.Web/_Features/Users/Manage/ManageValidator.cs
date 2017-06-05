using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.Users.Manage
{
    public interface IManageValidator
    {
        ValidationResult IsImage(IFormFile file);
    }

    public class ManageValidator : IManageValidator
    {
        public const int ImageMinimumBytes = 512;
        public const int ImageMaximumBytes = 1048576;

        public ValidationResult IsImage(IFormFile file)
        {
            var validationresult = new ValidationResult();

            if (file.ContentType.ToLower() != "image/jpg" &&
                file.ContentType.ToLower() != "image/jpeg" &&
                file.ContentType.ToLower() != "image/pjpeg" &&
                file.ContentType.ToLower() != "image/gif" &&
                file.ContentType.ToLower() != "image/x-png" &&
                file.ContentType.ToLower() != "image/png")
            {
                return validationresult.AddError("IsImage", "This is not an image file");
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
                && Path.GetExtension(file.FileName).ToLower() != ".png"
                && Path.GetExtension(file.FileName).ToLower() != ".gif"
                && Path.GetExtension(file.FileName).ToLower() != ".jpeg")
            {
                return validationresult.AddError("IsImage", "This is not an image file extension");
            }

            try
            {
                if (!file.OpenReadStream().CanRead)
                {
                    return validationresult.AddError("IsImage", "The image file is not readable");
                }

                if (file.Length < ImageMinimumBytes)
                {
                    return validationresult.AddError("IsImage", "This is not an image file");
                }

                if (file.Length > ImageMaximumBytes)
                {
                    return validationresult.AddError("IsImage", "The limit for profile images is 1 MB");
                }
            }
            catch (Exception)
            {
                return validationresult.AddError("IsImage", "This is not an image file");
            }

            return validationresult;
        }
    }
}
