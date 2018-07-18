using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Web._Features.Projects.Repository
{
    public class FileHistoryViewModel
    {
        public IEnumerable<FileItemHistoryViewModel> Files { get; set; }
        public string ProjectName { get; set; }
        public string FileName { get; set; }
    }

    public class FileItemHistoryViewModel : FileViewModelBase
    {
        public Guid EventId { get; set; }
        public DateTimeOffset FileAddedOn { get; set; }
        public long FileSize { get; set; }
        public long FileSizeInKbs
        {
            get
            {
                return FileSize / 1000;
            }
        }
    }
}
