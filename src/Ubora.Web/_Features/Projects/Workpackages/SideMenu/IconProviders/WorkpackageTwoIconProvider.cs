namespace Ubora.Web._Features.Projects.Workpackages.SideMenu.IconProviders
{
    public class WorkpackageTwoIconProvider : WorkpackageIconProviderBase
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/two_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/two_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/two_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/two_checked.svg" };
    }
}