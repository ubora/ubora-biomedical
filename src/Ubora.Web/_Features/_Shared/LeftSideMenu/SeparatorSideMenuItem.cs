namespace Ubora.Web._Features._Shared.LeftSideMenu
{
    // TODO: Because it does not fit the interface at all, do something about it. Usage is too procedural as well... But it's only used in a single place for now (resources menu).
    public class SeparatorSideMenuItem : ISideMenuItem
    {
        public string Id { get; } = "";
        public string DisplayName { get; } = "";
        public NestingLevel Nesting { get; } = NestingLevel.None;
        public string ATagClass { get; } = "";
        public bool IsSelected { get; } = false;
    }
}