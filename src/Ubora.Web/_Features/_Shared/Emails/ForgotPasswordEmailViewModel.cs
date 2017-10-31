using System;

namespace Ubora.Web._Features._Shared.Emails
{
    public class ForgotPasswordEmailViewModel : EmailLayoutViewModel
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}