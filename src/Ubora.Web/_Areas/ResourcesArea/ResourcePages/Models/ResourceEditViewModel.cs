using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Ubora.Domain.Resources;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class ResourceEditViewModel : ResourceEditPostModel
    {
        public class Factory
        {
            private readonly INodeServices _nodeServices;

            public Factory(INodeServices nodeServices)
            {
                _nodeServices = nodeServices;
            }

            public async Task<ResourceEditViewModel> Create(ResourcePage resourcePage)
            {
                return new ResourceEditViewModel
                {
                    ResourceId = resourcePage.Id,
                    Body =  await _nodeServices.InvokeAsync<string>("./Scripts/backend/SanitizeQuillDelta.js", resourcePage.Body.Value),
                    Title = resourcePage.Title,
                    ContentVersion = resourcePage.BodyVersion,
                    MenuPriority = resourcePage.MenuPriority,
                    ParentCategoryId = resourcePage.CategoryId
                }; 
            }
        }
    }
}