using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubora.Web._Features.Projects.Repository
{
    public class FileHistoryViewModel
    {
        public IEnumerable<FileItemHistoryViewModel> Files { get; set; }
        public string ProjectName { get; set; }
        public string FileName { get; set; }
    }

    public class FileItemHistoryViewModel
    {
        public string Comment { get; set; }
        public DateTimeOffset FileAddedOn { get; set; }
        public long FileSize { get; set; }
        public string DownloadUrl { get; set; }
    }
}
