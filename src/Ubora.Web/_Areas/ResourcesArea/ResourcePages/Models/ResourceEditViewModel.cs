using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Events;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class ResourceEditViewModel : ResourceEditPostModel
    {
        public class Factory
        {
            public ResourceEditViewModel Create(ResourcePage resourcePage)
            {
                return new ResourceEditViewModel
                {
                    ResourceId = resourcePage.Id,
                    Body = resourcePage.Body.Value,
                    Title = resourcePage.Title,
                    ContentVersion = resourcePage.BodyVersion
                };
            }
        }
    }
}