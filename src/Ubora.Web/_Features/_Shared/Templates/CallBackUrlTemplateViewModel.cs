using System;

namespace Ubora.Web._Features._Shared.Templates
{
    public class CallBackUrlTemplateViewModel
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public string UboraLogoContentId { get; set; }
        public string FacebookLogoContentId { get; set; }
        public string TwitterLogoContentId { get; set; }
        public string EmailLogoContentId { get; set; }
    }
}
