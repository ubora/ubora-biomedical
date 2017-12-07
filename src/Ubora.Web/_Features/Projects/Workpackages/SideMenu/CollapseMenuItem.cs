using System;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Projects.Workpackages.Queries;
using Ubora.Web._Features.Projects.Workpackages.SideMenu.IconProviders;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class CollapseMenuItem : ISideMenuItem
    {
        private readonly IIconProvider _iconProvider;

        public CollapseMenuItem(NestingLevel nesting, string id, string displayName, ISideMenuItem[] innerMenuItems, IIconProvider iconProvider = null)
        {
            Id = id;
            DisplayName = displayName;
            _iconProvider = iconProvider ?? new DefaultCollapseMenuItemIconProvider();
            Nesting = nesting;
            InnerMenuItems = innerMenuItems;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public ISideMenuItem[] InnerMenuItems { get; }
        public ImgIcon Icon => _iconProvider.ProvideIcon(this);
        public string InnerMenuItemsId => $"{Id}-items";

        public string CssClassForItemsBelow
        {
            get
            {
                switch (Nesting)
                {
                    case NestingLevel.None:
                        return "side-menu-secondary";
                    case NestingLevel.One:
                        return "side-menu-tertiary";
                    case NestingLevel.Two:
                        return "side-menu-quaternary";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public NestingLevel Nesting { get; set; }
        public WorkpackageStatus Status { get; set; }
        public bool IsSelected => InnerMenuItems.Any(item => item.IsSelected);
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
    }
}