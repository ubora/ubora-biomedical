using Microsoft.AspNetCore.Http;
using System;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Repository
{
    public class UpdateFileViewModel
    {
        [FileSize(4000000)]
        public IFormFile ProjectFile { get; set; }
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
    }
}
