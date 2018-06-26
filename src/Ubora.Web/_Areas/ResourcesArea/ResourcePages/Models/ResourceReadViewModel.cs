using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Ubora.Domain.Resources;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class ResourceReadViewModel
    {
        public Guid ResourceId { get; private set; }
        public string Title { get; private set; }
        public string BodyHtml { get; private set; }

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
                    ResourceId = resourcePage.Id,
                    BodyHtml = await _nodeServices.InvokeExportAsync<string>("./Scripts/backend/app-backend", "convertQuillDeltaToHtml", resourcePage.Content.Body.Value),
                    Title = resourcePage.Content.Title
                };
            }
        }
    }
}