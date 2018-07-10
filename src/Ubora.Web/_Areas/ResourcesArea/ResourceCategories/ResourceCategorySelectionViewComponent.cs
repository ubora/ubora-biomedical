using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Resources;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories
{
    public class ResourceCategorySelectionViewComponent : ViewComponent
    {
        private readonly IQueryProcessor _queryProcessor;

        public ResourceCategorySelectionViewComponent(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var allCategories = _queryProcessor.Find(new MatchAll<ResourceCategory>()).ToList();

            var rootCategories = allCategories.Where(c => !c.ParentCategoryId.HasValue);
            var parentCategoryIdFromModelStateAsString = ModelState["ParentCategoryId"]?.RawValue as string;
            var selectedValue = parentCategoryIdFromModelStateAsString != null ? Guid.Parse(parentCategoryIdFromModelStateAsString) : (Guid?)null;

            string html = @"
<select class=""form-control"" id=""ParentCategoryId"" name=""ParentCategoryId"">
<option value=""""></option>";
            foreach (var category in rootCategories)
            {
                html = html + CreateRadioButtonHtml(category, "", allCategories, selectedValue);
            }

            html = html + "</select>";

            return Task.FromResult<IViewComponentResult>(View("~/_Components/_HtmlRaw.cshtml", html));
        }

        private string CreateRadioButtonHtml(ResourceCategory category, string parentCategoryPrefix, IReadOnlyCollection<ResourceCategory> allCategories, Guid? selectedValue)
        {
            var selected = (selectedValue == category.Id) ? "selected=\"selected\"" : "";

            var html = $@"<option value=""{category.Id}"" {selected}>{parentCategoryPrefix + category.Title}</option>";

            foreach (var childCategory in allCategories.Where(c => c.ParentCategoryId == category.Id))
            {
                html = html + CreateRadioButtonHtml(childCategory, parentCategoryPrefix + category.Title + " / ", allCategories, selectedValue);
            };

            return html;
        }
    }
}
