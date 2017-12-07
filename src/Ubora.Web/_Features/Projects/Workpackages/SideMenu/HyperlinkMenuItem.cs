using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class HyperlinkMenuItem : ISideMenuItem
    {
        private readonly IIconProvider _iconProvider;

        public HyperlinkMenuItem(NestingLevel nesting, string id, string displayName, string href, IIconProvider iconProvider = null)
        {
            DisplayName = displayName;
            Href = href;
            Nesting = nesting;
            Id = id;
            _iconProvider = iconProvider ?? new DefaultHyperlinkMenuItemIconProvider();
        }

        public string Id { get; }
        public string DisplayName { get; }
        public bool IsSelected { get; set; }
        public string Href { get; }
        public NestingLevel Nesting { get; set; }
        public WorkpackageStatus Status { get; set; }

        public ImgIcon Icon => _iconProvider.Generate(this);

        public IHtmlContent GenerateHtmlMarkup(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Projects/Workpackages/SideMenu/_LinkMenuItemPartial.cshtml", model: this);
        }

        public ISideMenuItem SetStatus(WorkpackageStatus status)
        {
            Status = status;

            return this;
        }

        public string ATagClass
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
}