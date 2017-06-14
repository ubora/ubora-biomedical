using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Ubora.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class IsImageAttribute : ValidationAttribute
    {
        public const int ImageMinimumBytes = 512;
        public const int ImageMaximumBytes = 1048576;

        public IsImageAttribute() : base("This is not an image file!")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (IFormFile)value;

            if (file == null)
            {
                return new ValidationResult("Please select an image to upload first!");
            }

            if (file.ContentType.ToLower() != "image/jpg" &&
                file.ContentType.ToLower() != "image/jpeg" &&
                file.ContentType.ToLower() != "image/pjpeg" &&
                file.ContentType.ToLower() != "image/gif" &&
                file.ContentType.ToLower() != "image/x-png" &&
                file.ContentType.ToLower() != "image/png")
            {
                return new ValidationResult(ErrorMessage);
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
                && Path.GetExtension(file.FileName).ToLower() != ".png"
                && Path.GetExtension(file.FileName).ToLower() != ".gif"
                && Path.GetExtension(file.FileName).ToLower() != ".jpeg")
            {
                return new ValidationResult("This is not an image file extension");
            }

            try
            {
                if (!file.OpenReadStream().CanRead)
                {
                    return new ValidationResult("The image file is not readable");
                }

                if (file.Length < ImageMinimumBytes)
                {
                    return new ValidationResult(ErrorMessage);
                }

                if (file.Length > ImageMaximumBytes)
                {
                    return new ValidationResult("The limit for profile images is 1 MB");
                }
            }
            catch (Exception)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
