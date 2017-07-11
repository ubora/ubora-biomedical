using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Admin
{
    [Authorize(Roles = ApplicationRole.Admin)] // Important!
    public class AdminController : UboraController
    {
        private readonly ApplicationUserManager _userManager;

        public AdminController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> Diagnostics()
        {
            var userViewModels = new List<UserViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                userViewModels.Add(new UserViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return View(nameof(Diagnostics), userViewModels);
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> AddAdministratorRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.AddToRoleAsync(user, ApplicationRole.Admin);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await Diagnostics();
            }

            return RedirectToAction(nameof(Diagnostics));
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> RemoveAdministratorRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.RemoveFromRoleAsync(user, ApplicationRole.Admin);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await Diagnostics();
            }

            return RedirectToAction(nameof(Diagnostics));
        }

        private void AddIdentityErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
