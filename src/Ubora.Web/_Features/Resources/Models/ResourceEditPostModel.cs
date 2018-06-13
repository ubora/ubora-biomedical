using System;

namespace Ubora.Web._Features.Resources.Models
{
    public class ResourceEditPostModel
    {
        public Guid ResourceId { get; set; }
        public string Body { get; set; }
        public Guid ContentVersion { get; set; }
    }
}