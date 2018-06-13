using System;

namespace Ubora.Web._Features.Resources.Models
{
    public class ResourceReadViewModel
    {
        public Guid ResourceId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}