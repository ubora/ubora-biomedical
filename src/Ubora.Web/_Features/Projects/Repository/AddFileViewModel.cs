using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Repository
{
    public class AddFileViewModel
    {
        [FileSize(4000000)]
        public IFormFile ProjectFile { get; set; }
        public string ActionName { get; set; }
        public Guid FileId { get; set; }
        public string FileName
        {
            get
            {
                if(ProjectFile != null)
                {
                    var filePath = ProjectFile.FileName.Replace(@"\", "/");
                    var fileName = Path.GetFileName(filePath);
                    return fileName;
                }

                return "";
            }
        }
    }
}
