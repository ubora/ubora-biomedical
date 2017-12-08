namespace Ubora.Web._Features.Projects.Workpackages.SideMenu.IconProviders
{
    public class WorkpackageThreeIconProvider : WorkpackageIconProviderBase
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/three_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/three_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/three_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/three_checked.svg" };
    }
}