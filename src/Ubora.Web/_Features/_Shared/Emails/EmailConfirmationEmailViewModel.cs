using System;

namespace Ubora.Web._Features._Shared.Emails
{
    public class EmailConfirmationEmailViewModel : EmailLayoutViewModel
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}
