using Ubora.Domain.Projects.Workpackages.Queries;
using Ubora.Web._Features._Shared.LeftSideMenu;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class WpSideMenuHyperlinkMenuItem : HyperlinkMenuItem, IWorkpackageSideMenuItem
    {
        private readonly string _href;

        public WpSideMenuHyperlinkMenuItem(NestingLevel nesting, string id, string displayName, string href) 
        {
            DisplayName = displayName;
            _href = href;
            Nesting = nesting;
            Id = id;
        }

        public WorkpackageStatus Status { get; set; }

        public IWorkpackageSideMenuItem SetStatus(WorkpackageStatus status)
        {
            Status = status;
            return this;
        }

        public override string Href
        {
            get
            {
                if (Status == WorkpackageStatus.Closed)
                {
                    return "#";
                }
                return _href;
            }
        }

        public override string ATagClass
        {
            get
            {
                var isSelected = IsSelected ? "active-status " : "";

                switch (Status)
                {
                    case WorkpackageStatus.Unlockable:
                        return isSelected + "unlockable-status";
                    case WorkpackageStatus.Accepted:
                        return isSelected + "checked-status";
                    case WorkpackageStatus.Closed:
                        return isSelected + "muted-status";
                    default:
                        return isSelected;
                }
            }
        }
    }
}