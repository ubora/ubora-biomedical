using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Marten.Pagination;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources;
using Ubora.Web._Features._Shared.Paging;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class IndexResourceFilesViewModel
    {
        public IndexResourceFilesViewModel(IPagedList<ResourceFileListItemViewModel> files, Guid resourcePageId, string resourcePageName)
        {
            Pager = Pager.From(files);
            Files = files.ToList();
            ResourcePageId = resourcePageId;
            ResourcePageTitle = resourcePageName;
        }

        public Pager Pager { get; }
        public IReadOnlyCollection<ResourceFileListItemViewModel> Files { get; }
        public Guid ResourcePageId { get; }
        public string ResourcePageTitle { get; }
    }

    public class ResourceFileListItemViewModel
    {
        public ResourceFileListItemViewModel(Guid fileId, string fileName, long fileSize)
        {
            FileId = fileId;
            FileName = fileName;
            FileSize = fileSize;
        }

        public Guid FileId { get; }
        public string FileName { get; }
        public long FileSize { get; }
        public long FileSizeInKbs => FileSize / 1000;

        public class Projection : Projection<ResourceFile, ResourceFileListItemViewModel>
        {
            protected override Expression<Func<ResourceFile, ResourceFileListItemViewModel>> ToSelector()
            {
                return resourceFile => new ResourceFileListItemViewModel(resourceFile.Id, resourceFile.FileName, resourceFile.FileSize);
            }
        }
    }
}
