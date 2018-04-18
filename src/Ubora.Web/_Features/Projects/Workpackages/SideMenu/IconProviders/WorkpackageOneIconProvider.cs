namespace Ubora.Web._Features.Projects.Workpackages.SideMenu.IconProviders
{
    public class WorkpackageOneIconProvider : WorkpackageIconProviderBase
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/one_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/one_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/one_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/one_checked.svg" };
    }
}