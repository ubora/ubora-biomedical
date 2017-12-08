namespace Ubora.Web._Features.Projects.Workpackages.SideMenu.IconProviders
{
    public class WorkpackageSixIconProvider : WorkpackageIconProviderBase
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/six_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/six_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/six_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/six_checked.svg" };
    }
}