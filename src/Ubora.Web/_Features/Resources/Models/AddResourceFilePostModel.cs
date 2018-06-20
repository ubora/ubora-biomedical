using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Resources.Models
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
