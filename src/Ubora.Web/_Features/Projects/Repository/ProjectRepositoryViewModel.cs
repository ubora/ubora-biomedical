using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Web._Features.Projects.Repository
{
    public class ProjectRepositoryViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public AddFileViewModel AddFileViewModel { get; set; }
        public bool IsProjectLeader { get; set; }
        public IEnumerable<IGrouping<string, ProjectFileViewModel>> AllFiles { get; set; }
    }

    public class ProjectFileViewModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
        public long FileSize { get; set; }
        public int RevisionNumber { get; set; }
        public long FileSizeInKbs
        {
            get
            {
                return FileSize / 1000;
            }
        }
    }
}
