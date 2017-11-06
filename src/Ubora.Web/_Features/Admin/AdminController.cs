using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Users.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Admin
{
    [Authorize(Roles = ApplicationRole.Admin)]
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
            var user = await _userManager.FindByIdAsync(userId);
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
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.RemoveFromRoleAsync(user, ApplicationRole.Admin);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await Diagnostics();
            }

            return RedirectToAction(nameof(Diagnostics));
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> AddMentorRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddToRoleAsync(user, ApplicationRole.Mentor);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await Diagnostics();
            }

            return RedirectToAction(nameof(Diagnostics));
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> RemoveMentorRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.RemoveFromRoleAsync(user, ApplicationRole.Mentor);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await Diagnostics();
            }

            return RedirectToAction(nameof(Diagnostics));
        }

        [Route(nameof(DeleteUser))]
        public IActionResult DeleteUser(string userEmail)
        {
            var model = new UserViewModel
            {
                UserEmail = userEmail
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        [Route(nameof(DeleteUser))]
        public async Task<IActionResult> DeleteUser(DeleteUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Diagnostics));
            }

            var user = await _userManager.FindByEmailAsync(model.UserEmail);

            if (user == null)
            {
                return RedirectToAction(nameof(Diagnostics));
            }

            ExecuteUserCommand(new DeleteUserCommand
            {
                UserId = user.Id
            });

            await _userManager.DeleteAsync(user);

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Diagnostics));
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