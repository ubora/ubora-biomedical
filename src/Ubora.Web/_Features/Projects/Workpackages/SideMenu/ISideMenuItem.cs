using Ubora.Domain.Projects.Workpackages.Queries;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public interface ISideMenuItem
    {
        string Id { get; }
        string DisplayName { get; }
        NestingLevel Nesting { get; }
        string ATagClass { get;  }
        bool IsSelected { get; }
    }

    public interface IWorkpackageSideMenuItem : ISideMenuItem
    {

        /// <remarks>Return object itself for fluent-API.</remarks>>
        IWorkpackageSideMenuItem SetStatus(WorkpackageStatus status);
        WorkpackageStatus Status { get; }
    }
}
