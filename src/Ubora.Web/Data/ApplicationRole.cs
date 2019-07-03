using System;
using Microsoft.AspNetCore.Identity;

namespace Ubora.Web.Data
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public const string Admin = "Ubora.Administrator";
        public const string Mentor = "Ubora.Mentor";
        public const string ManagementGroup = "Ubora.ManagementGroup";

        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}