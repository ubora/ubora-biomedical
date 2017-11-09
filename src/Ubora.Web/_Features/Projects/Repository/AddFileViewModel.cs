using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Repository
{
    public class AddFileViewModel
    {
        [FileSize(4000000)]
        public IEnumerable<IFormFile> ProjectFiles { get; set; }
        [Required]
        public string FolderName { get; set; }
        public string Comment { get; set; }
    }
}

