using Ubora.Domain.Projects.Workpackages.Queries;
using Ubora.Web._Features._Shared.LeftSideMenu;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class WpSideMenuHyperlinkMenuItem : HyperlinkMenuItem, IWorkpackageSideMenuItem
    {
        public WpSideMenuHyperlinkMenuItem(NestingLevel nesting, string id, string displayName, string href) 
            : base(nesting, id, displayName, href)
        {
        }

        public WorkpackageStatus Status { get; set; }

        public IWorkpackageSideMenuItem SetStatus(WorkpackageStatus status)
        {
            Status = status;
            return this;
        }

        public override string ATagClass
        {
            get
            {
                if (IsSelected && Status == WorkpackageStatus.Unlockable)
                {
                    return "active-status unlockable-status";
                }

                if (IsSelected)
                {
                    return "active-status";
                }

                switch (Status)
                {
                    case WorkpackageStatus.Unlockable:
                        return "unlockable-status";
                    case WorkpackageStatus.Accepted:
                        return "checked-status";
                    case WorkpackageStatus.Closed:
                        return "muted-status";
                    default:
                        return "";
                }
            }
        }
    }
}