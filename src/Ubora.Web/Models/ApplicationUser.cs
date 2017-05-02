using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ubora.Web.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
    }

    public class ApplicationRole : IdentityRole<Guid>
    {
    }
}
