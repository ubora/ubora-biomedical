using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Web._Features._Shared.LeftSideMenu
{
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