using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Resources;
using Ubora.Web._Areas.ResourcesArea.ResourcePages;
using Ubora.Web._Features._Shared.LeftSideMenu;

namespace Ubora.Web._Areas.ResourcesArea._Shared.Models
{
    public class ResourcesHierarchySideMenuFactory
    {
        private readonly IUrlHelper _urlHelper;

        public ResourcesHierarchySideMenuFactory(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public IEnumerable<ISideMenuItem> CreateSideMenuItems(ResourcesMenu root)
        {
            var rootLinks = root.Links.Where(link => !link.ParentCategoryId.HasValue);

            foreach (var sideMenuItem in CreateSideMenuItems(rootLinks, 0, root))
            {
                yield return sideMenuItem;
            }
        }

        public IEnumerable<ISideMenuItem> CreateSideMenuItems(IEnumerable<IResourceMenuLink> links, int nesting, ResourcesMenu root)
        {
            foreach (var link in links)
            {
                switch (link)
                {
                    case ResourcePageLink _:
                        yield return new ResourcesSideMenuHyperlinkMenuItem((NestingLevel)nesting, link.Id.ToString(), link.Title, _urlHelper.Action(nameof(ResourcePagesController.Read), nameof(ResourcePagesController).RemoveSuffix(), new { resourcePageId = link.Id }));
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