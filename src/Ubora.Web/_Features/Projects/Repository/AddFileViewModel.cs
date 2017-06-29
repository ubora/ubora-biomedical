using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Repository
{
    public class AddFileViewModel
    {
        [FileSize(4000000)]
        public IFormFile ProjectFile { get; set; }
    }
}
