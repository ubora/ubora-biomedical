using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class ResourceIndexViewModel
    {
        public Guid ResourceId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }

        public class Mapper : Projection<ResourcePage, ResourceIndexViewModel>
        {
            protected override Expression<Func<ResourcePage, ResourceIndexViewModel>> ToSelector()
            {
                return resource => new ResourceIndexViewModel
                {
                    ResourceId = resource.Id,
                    Title = resource.Content.Title,
                    Slug = resource.ActiveSlug.Value
                };
            }
        }
    }
}