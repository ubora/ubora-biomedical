using System.Collections.Generic;
using Ubora.Web._Features._Shared.LeftSideMenu;

namespace Ubora.Web._Areas.ResourcesArea._Shared.Models
{
    public class ResourcesSideMenuCollapseMenuItem : CollapseMenuItem
    {
        public ResourcesSideMenuCollapseMenuItem(NestingLevel nesting, string id, string displayName, IEnumerable<ISideMenuItem> innerMenuItems) 
            : base(nesting, id, displayName, innerMenuItems)
        {
        }
    }
}