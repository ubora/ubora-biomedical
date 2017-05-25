using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfileController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult View(Guid userId)
        {
            var profileViewModel = new ProfileViewModel();

            return View(profileViewModel);
        }
    }
}