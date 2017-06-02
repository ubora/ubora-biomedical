using System.IO;
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
                return validationresult.AddError("IsImage", "This is not an image file");
            }

            return validationresult;
        }
    }
}
