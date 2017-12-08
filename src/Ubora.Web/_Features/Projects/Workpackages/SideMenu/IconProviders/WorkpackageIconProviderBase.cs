using System;
using Ubora.Domain.Projects.Workpackages.Queries;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu.IconProviders
{
    public abstract class WorkpackageIconProviderBase : IIconProvider
    {
        public abstract ImgIcon Selected { get; }
        public abstract ImgIcon Closed { get; }
        public abstract ImgIcon Opened { get; }
        public abstract ImgIcon Accepted { get; }

        public ImgIcon ProvideIcon(ISideMenuItem item)
        {
            switch (item.Status)
            {
                case WorkpackageStatus.Closed:
                    return Closed;
                case WorkpackageStatus.Opened:
                    return Opened;
                case WorkpackageStatus.Accepted:
                    return Accepted;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}