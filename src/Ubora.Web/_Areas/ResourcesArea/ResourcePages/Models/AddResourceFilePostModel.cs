using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class AddResourceFileViewModel : AddResourceFilePostModel
    {
        public AddResourceFileViewModel(Guid resourcePageId, string resourcePageTitle)
        {
            ResourcePageId = resourcePageId;
            ResourcePageTitle = resourcePageTitle;
        }

        public string ResourcePageTitle { get; }
    }

    public class AddResourceFilePostModel
    {
        public Guid ResourcePageId { get; set; }
        [FileSize(4000000)]
        public IEnumerable<IFormFile> ProjectFiles { get; set; }
    }
}
