using Microsoft.AspNetCore.Http;
using System.IO;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class EditProjectImageViewModel
    {
        [IsImage]
        public IFormFile ProjectImage { get; set; }
        private string FilePath => ProjectImage.FileName.Replace(@"\\", "/");
        public string FileName => Path.GetFileName(FilePath);
    }
}
