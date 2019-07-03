using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Specifications;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories
{
    public class ResourceCategorySelectOptionsViewComponent : ViewComponent
    {
        private readonly IQueryProcessor _queryProcessor;

        public ResourceCategorySelectOptionsViewComponent(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        /// <param name="removedCategory">Note that children categories will not be _displayed_ either.</param>
        public Task<IViewComponentResult> InvokeAsync(Guid? selectedCategory, Guid? removedCategory)
        {
            Specification<ResourceCategory> resourceCategorySpec = new MatchAll<ResourceCategory>();
            if (removedCategory.HasValue)
            {
                resourceCategorySpec = !new HasIdResourceCategorySpec(removedCategory.Value);
            }

            return Task.FromResult<IViewComponentResult>(View("~/_Areas/ResourcesArea/ResourceCategories/_FormSelectOptions.cshtml", new FormSelectOptionsViewModel
            {
                Categories = _queryProcessor.Find(resourceCategorySpec).ToList(), // TODO: Don't pass entities to view.
                SelectedCategory = selectedCategory,
                RemovedCategory = removedCategory
            }));
        }
    }
}
