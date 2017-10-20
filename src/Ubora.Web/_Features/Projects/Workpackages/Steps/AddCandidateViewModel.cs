using Microsoft.AspNetCore.Http;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class AddCandidateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [IsImage]
        [FileSize(4000000)]
        public IFormFile Image { get; set; }
    }
}