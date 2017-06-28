namespace Ubora.Web
{
    public class SmtpSettings
    {
        public bool UseSpecifiedPickupDirectory { get; set; }
        public string EmailPickupDirectory { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpHostname { get; set; }
        public int SmtpPort { get; set; }
    }
}
