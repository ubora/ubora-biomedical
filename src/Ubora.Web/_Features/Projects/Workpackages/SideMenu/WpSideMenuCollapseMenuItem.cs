using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Queries;
using Ubora.Web._Features._Shared.LeftSideMenu;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class WpSideMenuCollapseMenuItem : CollapseMenuItem, IWorkpackageSideMenuItem
    {
        public WpSideMenuCollapseMenuItem(NestingLevel nesting, string id, string displayName, IEnumerable<IWorkpackageSideMenuItem> innerMenuItems)
            : base(nesting, id, displayName, innerMenuItems)
        {
        }

        public WorkpackageStatus Status { get; private set; }

        public IWorkpackageSideMenuItem SetStatus(WorkpackageStatus status)
        {
            foreach (var innerMenuItem in InnerMenuItems.Cast<IWorkpackageSideMenuItem>())
            {
                Status = status;

                if (status == WorkpackageStatus.Accepted)
                {
                    // Ugly solution.
                    innerMenuItem.SetStatus(WorkpackageStatus.Opened);
                }
                else
                {
                    innerMenuItem.SetStatus(status);
                }
            }
            return this;
        }

        public override string ATagClass
        {
            get
            {
                switch (Status)
                {
                    case WorkpackageStatus.Accepted:
                        return "checked-status";
                    case WorkpackageStatus.Closed:
                        return "muted-status";
                    default:
                        return "";
                }
            }
        }

        public override bool RenderInnerMenuItems => Status != WorkpackageStatus.Closed;
    }
}