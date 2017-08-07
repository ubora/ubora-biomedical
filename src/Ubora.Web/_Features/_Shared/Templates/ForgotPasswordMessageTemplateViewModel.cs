using System;

namespace Ubora.Web._Features._Shared.Templates
{
    public class ForgotPasswordMessageTemplateViewModel
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}
