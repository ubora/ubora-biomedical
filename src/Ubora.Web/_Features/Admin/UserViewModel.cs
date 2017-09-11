using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Admin
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }

        public string UserEmail { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public bool IsAdmin => Roles.Any(x => x == ApplicationRole.Admin);
        public bool IsMentor => Roles.Any(x => x == ApplicationRole.Mentor);
    }
}