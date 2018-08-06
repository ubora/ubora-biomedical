using Ubora.Web._Features._Shared.LeftSideMenu;

namespace Ubora.Web._Areas.ResourcesArea._Shared.Models
{
    public class ResourcesSideMenuHyperlinkMenuItem : HyperlinkMenuItem
    {
        public ResourcesSideMenuHyperlinkMenuItem(NestingLevel nesting, string id, string displayName, string href) 
        {
            DisplayName = displayName;
            Href = href;
            Nesting = nesting;
            Id = id;
        }
    }
}