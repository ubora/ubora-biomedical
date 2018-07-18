using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories.Models
{
    public class ListResourceCategoryViewModel
    {
        public IReadOnlyCollection<ListItemResourceCategoryViewModel> Categories { get; set; }
    }

    public class ListItemResourceCategoryViewModel
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public Guid? ParentCategoryId { get; set; }

        public class Projection : Projection<ResourceCategory, ListItemResourceCategoryViewModel>
        {
            protected override Expression<Func<ResourceCategory, ListItemResourceCategoryViewModel>> ToSelector()
            {
                return resourceCategory => new ListItemResourceCategoryViewModel
                {
                    CategoryId = resourceCategory.Id,
                    Title = resourceCategory.Title,
                    ParentCategoryId = resourceCategory.ParentCategoryId
                };
            }
        }
    }
}
