using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Repository
{
    public class ProjectRepositoryViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IFormFile ProjectFile { get; set; }
        public IEnumerable<RepositoryListItemViewModel> Files { get; set; }
    }

    public class RepositoryListItemViewModel
    {
        public string FileName { get; set; }
        public string FileLocation { get; set; }
    }
}
