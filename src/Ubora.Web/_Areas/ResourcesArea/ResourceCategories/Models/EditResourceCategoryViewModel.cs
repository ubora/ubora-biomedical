using Ubora.Domain.Resources;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories.Models
{
    public class EditResourceCategoryViewModel : EditResourceCategoryPostModel
    {
        public class Factory
        {
            public EditResourceCategoryViewModel Create(ResourceCategory category)
            {
                return new EditResourceCategoryViewModel
                {
                    Title = category.Title,
                    CategoryId = category.Id,
                    Description = category.Description,
                    MenuPriority = category.MenuPriority,
                    ParentCategoryId = category.ParentCategoryId,
                };
            }
        }
    }
}