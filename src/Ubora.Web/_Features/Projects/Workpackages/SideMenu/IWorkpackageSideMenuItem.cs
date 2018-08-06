using Ubora.Domain.Projects.Workpackages.Queries;
using Ubora.Web._Features._Shared.LeftSideMenu;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public interface IWorkpackageSideMenuItem : ISideMenuItem
    {
        /// <remarks>Return object itself for fluent-API.</remarks>>
        IWorkpackageSideMenuItem SetStatus(WorkpackageStatus status);
        WorkpackageStatus Status { get; }
    }
}