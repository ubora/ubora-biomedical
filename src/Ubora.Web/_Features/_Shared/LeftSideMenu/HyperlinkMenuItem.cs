namespace Ubora.Web._Features._Shared.LeftSideMenu
{
    public abstract class HyperlinkMenuItem : ISideMenuItem
    {
        public string Id { get; protected set; }
        public string DisplayName { get; protected set; }
        public bool IsSelected { get; set; }
        public virtual string Href { get; protected set; }
        public NestingLevel Nesting { get; protected set; }

        public virtual string ATagClass => IsSelected ? "active-status" : "";
    }
}