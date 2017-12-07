using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class CollapseMenuItem : ISideMenuItem
    {
        protected readonly IIconProvider _iconProvider;

        public CollapseMenuItem(NestingLevel nesting, string id, string displayName, ISideMenuItem[] innerMenuItems, IIconProvider iconProvider = null)
        {
            Id = id;
            DisplayName = displayName;
            _iconProvider = iconProvider ?? new DefaultCollapseMenuItemIconProvider();
            Nesting = nesting;
            InnerMenuItems = innerMenuItems;
        }

        protected CollapseMenuItem()
        {
        }

        public string Id { get; }
        public string DisplayName { get; }
        public ISideMenuItem[] InnerMenuItems { get; }
        public ImgIcon Icon => _iconProvider.Generate(this);
        public string InnerMenuItemsId => $"{Id}-items";

        public NestingLevel Nesting { get; set; }
        public WorkpackageStatus Status { get; set; }

        public bool IsSelected => InnerMenuItems.Any(item => item.IsSelected);

        public IHtmlContent GenerateHtmlMarkup(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Projects/Workpackages/SideMenu/_CategoryMenuItemPartial.cshtml", model: this);
        }

        public ISideMenuItem SetStatus(WorkpackageStatus status)
        {
            foreach (var innerMenuItem in InnerMenuItems)
            {
                Status = status;
                innerMenuItem.SetStatus(status);
            }
            return this;
        }

        public string ATagClass
        {
            get
            {
                switch (Status)
                {
                    case WorkpackageStatus.Accepted:
                        return "checked-status";
                    default:
                        return "";
                }
            }
        }
    }
}