namespace Ubora.Web._Features.Projects.Workpackages.SideMenu.IconProviders
{
    public class WorkpackageFiveIconProvider : WorkpackageIconProviderBase
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/five_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/five_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/five_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/five_checked.svg" };
    }
}