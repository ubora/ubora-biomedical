namespace Ubora.Web._Features._Shared.LeftSideMenu
{
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