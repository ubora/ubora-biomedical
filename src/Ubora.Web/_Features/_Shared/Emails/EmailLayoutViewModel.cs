using MimeKit;

namespace Ubora.Web._Features._Shared.Emails
{
    public abstract class EmailLayoutViewModel
    {
        public const string HeadingUboraLogoId = "heading-ubora-logo-id";
        public const string FooterEmailIconId = "footer-email-icon-id";
        public const string FooterFacebookLogoId = "footer-facebook-logo-id";
        public const string FooterTwitterLogoId = "footer-twitter-logo-id";

        public static void AddLayoutAttachments(AttachmentCollection linkedResources)
        {
            linkedResources.Add("./wwwroot/images/icon.png").ContentId = HeadingUboraLogoId;
            linkedResources.Add("./wwwroot/images/email_icon.png").ContentId = FooterEmailIconId;
            linkedResources.Add("./wwwroot/images/facebook_icon.png").ContentId = FooterFacebookLogoId;
            linkedResources.Add("./wwwroot/images/twitter_icon.png").ContentId = FooterTwitterLogoId;
        }
    }
}