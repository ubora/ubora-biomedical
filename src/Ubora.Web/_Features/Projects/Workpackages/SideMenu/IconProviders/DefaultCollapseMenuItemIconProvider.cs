using System;
using Ubora.Domain.Projects.Workpackages.Queries;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu.IconProviders
{
    public class DefaultCollapseMenuItemIconProvider : IIconProvider
    {
        public ImgIcon ProvideIcon(ISideMenuItem item)
        {
            switch (item.Status)
            {
                case WorkpackageStatus.Closed:
                    return new ImgIcon { Src = "/images/icons/circle-empty.svg" };
                case WorkpackageStatus.Opened:
                    return new ImgIcon { Src = "/images/icons/circle.svg" };
                case WorkpackageStatus.Accepted:
                    return new ImgIcon { Src = "/images/icons/check.svg" };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}