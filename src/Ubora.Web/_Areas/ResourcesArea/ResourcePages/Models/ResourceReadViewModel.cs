using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Events;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class ResourceReadViewModel
    {
        public Guid ResourceId { get; private set; }
        public string Title { get; private set; }
        public string ContentHtml { get; private set; }

        public class Factory
        {
            private readonly INodeServices _nodeServices;

            public Factory(INodeServices nodeServices)
            {
                _nodeServices = nodeServices;
            }

            protected Factory()
            {
            }

            public virtual async Task<ResourceReadViewModel> Create(ResourcePage resourcePage)
            {
                return new ResourceReadViewModel
                {
                    ContentHtml = await _nodeServices.InvokeAsync<string>("./Scripts/backend/ConvertQuillDeltaToHtml.js", resourcePage.Body.Value),
                    ResourceId = resourcePage.Id,
                    Title = resourcePage.Title
                };
            }
        }
    }
}