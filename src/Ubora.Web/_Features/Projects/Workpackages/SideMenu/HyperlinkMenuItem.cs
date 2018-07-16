using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Projects.Workpackages.Queries;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class HyperlinkMenuItem : ISideMenuItem
    {
        public HyperlinkMenuItem(NestingLevel nesting, string id, string displayName, string href)
        {
            DisplayName = displayName;
            Href = href;
            Nesting = nesting;
            Id = id;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public bool IsSelected { get; set; }
        public bool IsLocked { get; set; }
        public string Href { get; }
        public NestingLevel Nesting { get; set; }
        public WorkpackageStatus Status { get; set; }
        public string ATagClass
        {
            get
            {
                if (IsLocked)
                {
                    return "unlocked-status";
                }
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

        public IHtmlContent GenerateHtmlMarkup(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Projects/Workpackages/SideMenu/_LinkMenuItemPartial.cshtml", model: this);
        }

        public ISideMenuItem SetStatus(WorkpackageStatus status)
        {
            Status = status;

            return this;
        }
    }
}