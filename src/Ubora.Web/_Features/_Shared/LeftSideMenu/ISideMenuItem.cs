namespace Ubora.Web._Features._Shared.LeftSideMenu
{
    public interface ISideMenuItem
    {
        string Id { get; }
        string DisplayName { get; }
        NestingLevel Nesting { get; }
        string ATagClass { get;  }
        bool IsSelected { get; }
    }
}
