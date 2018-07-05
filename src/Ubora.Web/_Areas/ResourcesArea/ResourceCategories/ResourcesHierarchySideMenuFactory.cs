using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Resources;
using Ubora.Web._Areas.ResourcesArea.ResourcePages;
using Ubora.Web._Features.Projects.Workpackages.SideMenu;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories
{
    public class ResourcesSideMenuHyperlinkMenuItem : HyperlinkMenuItem
    {
        public ResourcesSideMenuHyperlinkMenuItem(NestingLevel nesting, string id, string displayName, string href) 
            : base(nesting, id, displayName, href)
        {
        }
    }

    public class ResourcesSideMenuCollapseMenuItem : CollapseMenuItem
    {
        public ResourcesSideMenuCollapseMenuItem(NestingLevel nesting, string id, string displayName, IEnumerable<ISideMenuItem> innerMenuItems) 
            : base(nesting, id, displayName, innerMenuItems)
        {
        }
    }

    public class ResourcesHierarchySideMenuFactory
    {
        private readonly IUrlHelper _urlHelper;

        public ResourcesHierarchySideMenuFactory(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public IEnumerable<ISideMenuItem> CreateSideMenuItems(ResourcesHierarchy root)
        {
            var rootLinks = root.Links.Where(link => !link.ParentCategoryId.HasValue);

            foreach (var sideMenuItem in CreateSideMenuItems(rootLinks, 0, root))
            {
                yield return sideMenuItem;
            }
        }

        public IEnumerable<ISideMenuItem> CreateSideMenuItems(IEnumerable<ILink> links, int nesting, ResourcesHierarchy root)
        {
            foreach (var link in links)
            {
                switch (link)
                {
                    case ResourcePageLink _:
                        yield return new ResourcesSideMenuHyperlinkMenuItem((NestingLevel)nesting, link.Slug.ToString(), link.Title, _urlHelper.Action(nameof(ResourcePagesController.Read), nameof(ResourcePagesController).RemoveSuffix(), new { slugOrId = link.Id }));
                        break;
                    case ResourceCategoryLink _:
                        var children = root.Links.Where(l => l.ParentCategoryId == link.Id).ToList();
                        if (children.Any())
                        {
                            yield return new ResourcesSideMenuCollapseMenuItem((NestingLevel)nesting, link.Id.ToString(), link.Title, CreateSideMenuItems(children, nesting + 1, root).ToList());
                        }
                        break;
                }
            }
        }

        public static bool MarkSelected(IEnumerable<ISideMenuItem> items, string selectedId)
        {
            foreach (var item in items)
            {
                var asCollapseMenuItem = item as CollapseMenuItem;
                if (asCollapseMenuItem != null)
                {
                    var wasSubItemSelected = MarkSelected(asCollapseMenuItem.InnerMenuItems, selectedId);
                    if (wasSubItemSelected)
                    {
                        return true;
                    }
                }
                else
                {
                    var isSelectedHyperlinkMenuItem = string.Equals(item.Id, selectedId, StringComparison.OrdinalIgnoreCase);
                    if (isSelectedHyperlinkMenuItem)
                    {
                        ((HyperlinkMenuItem)item).IsSelected = true;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}