using Ubora.Domain.Projects.Workpackages.Queries;

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
                if (IsSelected)
                {
                    return "active-status";
                }
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
    }

    public abstract class HyperlinkMenuItem : ISideMenuItem
    {
        protected HyperlinkMenuItem(NestingLevel nesting, string id, string displayName, string href)
        {
            DisplayName = displayName;
            Href = href;
            Nesting = nesting;
            Id = id;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public bool IsSelected { get; set; }
        public string Href { get; }
        public NestingLevel Nesting { get; }

        public virtual string ATagClass => IsSelected ? "active-status" : "";
    }
}