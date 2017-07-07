﻿using Microsoft.AspNetCore.Http;
using System.IO;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class EditProjectImageViewModel
    {
        [IsImage]
        [FileSize(4000000, "Max project image size is 4 MB!")]
        public IFormFile Image { get; set; }
        private string FilePath => Image.FileName.Replace(@"\\", "/");
        public string ImageName => Path.GetFileName(FilePath);
        public string ImageNameWithoutExtension => Path.GetFileNameWithoutExtension(FilePath);
    }
}
