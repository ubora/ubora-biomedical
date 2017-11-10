using Microsoft.AspNetCore.Http;
using System.IO;

namespace Ubora.Web.Infrastructure.Extensions
{
    public static class FileExtensions
    {
        public static string GetFileName(this IFormFile formFile)
        {
            if (formFile != null)
            {
                var filePath = formFile.FileName.Replace(@"\", "/");
                var fileName = Path.GetFileName(filePath);
                return fileName;
            }

            return "";
        }
    }
}
