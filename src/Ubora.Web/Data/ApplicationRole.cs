using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ubora.Web.Data
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public const string Admin = "Ubora.Administrator";
    }
}