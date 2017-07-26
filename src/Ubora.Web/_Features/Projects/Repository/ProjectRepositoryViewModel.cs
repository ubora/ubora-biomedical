using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Repository
{
    public class ProjectRepositoryViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public AddFileViewModel AddFileViewModel { get; set; }
        public IEnumerable<ProjectFileViewModel> Files { get; set; }
    }

    public class ProjectFileViewModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
    }
}
