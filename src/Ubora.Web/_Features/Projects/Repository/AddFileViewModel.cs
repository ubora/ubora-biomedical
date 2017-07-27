using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Repository
{
    public class AddFileViewModel
    {
        [FileSize(4000000)]
        public IFormFile ProjectFile { get; set; }
        public string ActionName { get; set; }
        public Guid FileId { get; set; }
    }
}
