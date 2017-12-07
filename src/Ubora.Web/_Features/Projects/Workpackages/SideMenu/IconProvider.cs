using System;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public interface IIconProvider
    {
        ImgIcon Generate(ISideMenuItem item);
    }

    public class DefaultHyperlinkMenuItemIconProvider : IIconProvider
    {
        public ImgIcon Generate(ISideMenuItem item)
        {
            if (item.IsSelected)
            {
                return new ImgIcon { Src = "/images/icons/arrow.svg" };
            }

            switch (item.Status)
            {
                case WorkpackageStatus.Closed:
                    return new ImgIcon { Src = "/images/icons/circle-empty.svg" };
                case WorkpackageStatus.Opened:
                    if (item.Nesting.Order == 3)
                    {
                        return new ImgIcon { Src = "/images/icons/circle-empty.svg" };
                    }
                    return new ImgIcon { Src = "/images/icons/circle.svg" };
                case WorkpackageStatus.Accepted:
                    return new ImgIcon { Src = "/images/icons/check.svg" };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class DefaultCollapseMenuItemIconProvider : IIconProvider
    {
        public ImgIcon Generate(ISideMenuItem item)
        {
            switch (item.Status)
            {
                case WorkpackageStatus.Closed:
                    return new ImgIcon { Src = "/images/icons/circle-empty.svg" };
                case WorkpackageStatus.Opened:
                    if (item.Nesting.Order == 3)
                    {
                        return new ImgIcon { Src = "/images/icons/circle-empty.svg" };
                    }
                    return new ImgIcon { Src = "/images/icons/circle.svg" };
                case WorkpackageStatus.Accepted:
                    return new ImgIcon { Src = "/images/icons/check.svg" };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public abstract class WorkpackageHeadingIconProvider : IIconProvider
    {
        public abstract ImgIcon Selected { get; }
        public abstract ImgIcon Closed { get; }
        public abstract ImgIcon Opened { get; }
        public abstract ImgIcon Accepted { get; }

        public ImgIcon Generate(ISideMenuItem item)
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

    public class WorkpackageSixIconProvider : WorkpackageHeadingIconProvider
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/six_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/six_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/six_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/six_checked.svg" };
    }

    public class WorkpackageFiveIconProvider : WorkpackageHeadingIconProvider
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/five_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/five_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/five_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/five_checked.svg" };
    }

    public class WorkpackageFourIconProvider : WorkpackageHeadingIconProvider
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/four_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/four_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/four_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/four_checked.svg" };
    }

    public class WorkpackageThreeIconProvider : WorkpackageHeadingIconProvider
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/three_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/three_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/three_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/three_checked.svg" };
    }

    public class WorkpackageTwoIconProvider : WorkpackageHeadingIconProvider
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/two_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/two_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/two_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/two_checked.svg" };
    }

    public class WorkpackageOneIconProvider : WorkpackageHeadingIconProvider
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/one_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/one_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/one_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/one_checked.svg" };
    }
}
