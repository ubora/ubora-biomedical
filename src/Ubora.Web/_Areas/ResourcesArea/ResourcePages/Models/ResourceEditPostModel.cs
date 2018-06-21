using System;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class ResourceEditPostModel
    {
        public Guid ResourceId { get; set; }
        public string Body { get; set; }
        public Guid ContentVersion { get; set; }
    }
}