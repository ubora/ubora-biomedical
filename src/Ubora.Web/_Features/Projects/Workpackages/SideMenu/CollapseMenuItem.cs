using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Queries;

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

    public abstract class CollapseMenuItem : ISideMenuItem
    {
        protected CollapseMenuItem(NestingLevel nesting, string id, string displayName, IEnumerable<ISideMenuItem> innerMenuItems)
        {
            Id = id;
            DisplayName = displayName;
            Nesting = nesting;
            InnerMenuItems = innerMenuItems;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public IEnumerable<ISideMenuItem> InnerMenuItems { get; }
        public string InnerMenuItemsId => $"{Id}-items";
        public virtual string ATagClass => "";

        public virtual string CssClassForItemsBelow
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

        public NestingLevel Nesting { get; }

        public bool IsSelected => InnerMenuItems.Any(item => item.IsSelected);
        public virtual bool RenderInnerMenuItems => true;
    }
}