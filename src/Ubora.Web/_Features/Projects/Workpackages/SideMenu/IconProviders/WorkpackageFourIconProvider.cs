namespace Ubora.Web._Features.Projects.Workpackages.SideMenu.IconProviders
{
    public class WorkpackageFourIconProvider : WorkpackageIconProviderBase
    {
        public override ImgIcon Selected => new ImgIcon { Src = "/images/icons/four_active.svg" };
        public override ImgIcon Closed => new ImgIcon { Src = "/images/icons/four_muted.svg" };
        public override ImgIcon Opened => new ImgIcon { Src = "/images/icons/four_open.svg" };
        public override ImgIcon Accepted => new ImgIcon { Src = "/images/icons/four_checked.svg" };
    }
}