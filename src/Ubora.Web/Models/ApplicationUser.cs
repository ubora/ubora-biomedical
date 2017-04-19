﻿using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ubora.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}
