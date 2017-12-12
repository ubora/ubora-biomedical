using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects._Queries;
using Ubora.Domain.Users.Commands;
using Ubora.Domain.Users.Queries;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Admin
{
    [Authorize(Roles = ApplicationRole.Admin)]
    public class AdminController : UboraController
    {
        private readonly IApplicationUserManager _userManager;


        public AdminController(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public IActionResult Diagnostics()
        {
            return View(nameof(Diagnostics));
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> ManageUsers()
        {
            var userViewModels = new List<UserViewModel>();

            IReadOnlyDictionary<Guid, string> userFullNames = QueryProcessor.ExecuteQuery(new FindFullNamesOfAllUboraUsersQuery());

            foreach (var user in _userManager.Users.ToList())
            {
                userViewModels.Add(new UserViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    FullName = userFullNames[user.Id],
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            var orderedViewModel = userViewModels.OrderBy(x => x.FullName).ToList();

            return View(nameof(ManageUsers), orderedViewModel);
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public IActionResult ProjectsUnderReview([FromServices] ProjectUnderReviewViewModel.Factory projectFactory)
        {
            var projectsUnderReview = QueryProcessor.ExecuteQuery(new GetProjectsUnderReviewQuery());

            var projectsViewModel = new ProjectsUnderReviewViewModel
            {
                ProjectsUnderReview = projectsUnderReview.Select(projectFactory.Create)
            };

            return View(nameof(ProjectsUnderReview), projectsViewModel);
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

                return Diagnostics();
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

                return Diagnostics();
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

                return Diagnostics();
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

                return Diagnostics();
            }

            return RedirectToAction(nameof(Diagnostics));
        }

        [Route(nameof(DeleteUser))]
        public IActionResult DeleteUser(string userEmail)
        {
            var model = new DeleteUserViewModel
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
                return Diagnostics();
            }

            var user = await _userManager.FindByEmailAsync(model.UserEmail);
            if (user == null)
            {
                ModelState.AddModelError("", $"User not found by e-mail: {model.UserEmail}");
                return Diagnostics();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                ExecuteUserCommand(new DeleteUserCommand
                {
                    UserId = user.Id
                });
            }
            else
            {
                AddIdentityErrorsToModelState(result);
            }

            if (!ModelState.IsValid)
            {
                return Diagnostics();
            }

            Notices.Success($"User {user.Email} successfully deleted");
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