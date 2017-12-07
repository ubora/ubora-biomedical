namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public class NestingLevel
    {
        public static NestingLevel None => new NestingLevel { Order = 0, CssClassForItemsBelow = "side-menu-secondary" };
        public static NestingLevel One => new NestingLevel { Order = 1, CssClassForItemsBelow = "side-menu-tertiary" };
        public static NestingLevel Two => new NestingLevel { Order = 2, CssClassForItemsBelow = "side-menu-quaternary" };
        public static NestingLevel Three => new NestingLevel { Order = 3, CssClassForItemsBelow = "" };

        public string CssClassForItemsBelow { get; set; }
        public int Order { get; set; }
    }
}