using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Resources;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories
{
    /// <summary>
    /// This component was made because of the recursive nature of this logic which didn't find a partial view.
    /// This component does not take selected value into account. I suggest you handle it client-side.
    /// </summary>
    public class ResourceCategorySelectOptionsViewComponent : ViewComponent
    {
        private readonly IQueryProcessor _queryProcessor;

        public ResourceCategorySelectOptionsViewComponent(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var allCategories = _queryProcessor.Find(new MatchAll<ResourceCategory>()).ToList();
            var rootCategories = allCategories.Where(c => c.ParentCategoryId == null);

            var htmlBuilder = new StringBuilder(@"<option value=""""></option>");
            foreach (var category in rootCategories)
            {
                CreateRadioButtonHtml(category, "", allCategories, htmlBuilder);
            }

            return Task.FromResult<IViewComponentResult>(View("~/_Components/_HtmlRaw.cshtml", htmlBuilder.ToString()));
        }

        private void CreateRadioButtonHtml(ResourceCategory category, string parentCategoryPrefix, IReadOnlyCollection<ResourceCategory> allCategories, StringBuilder htmlBuilder)
        {
            htmlBuilder.Append($@"<option value=""{category.Id}"">{parentCategoryPrefix + category.Title}</option>");

            foreach (var childCategory in allCategories.Where(c => c.ParentCategoryId == category.Id))
            {
                CreateRadioButtonHtml(childCategory, $"{parentCategoryPrefix}{category.Title}/", allCategories, htmlBuilder);
            }
        }
    }
}
