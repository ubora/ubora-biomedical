using System;

namespace Ubora.Web._Features._Shared.Emails
{
    public class ChangeEmailConfirmationViewModel : EmailLayoutViewModel
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public string NewEmail { get; set; }
    }
}
