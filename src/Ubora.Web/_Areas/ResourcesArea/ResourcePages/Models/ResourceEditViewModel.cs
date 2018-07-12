using Ubora.Domain.Resources;

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
                    ContentVersion = resourcePage.BodyVersion,
                    MenuPriority = resourcePage.MenuPriority,
                    ParentCategoryId = resourcePage.CategoryId
                }; 
            }
        }
    }
}