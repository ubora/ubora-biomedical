using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web._Features.Users.Manage
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }
    }
}
