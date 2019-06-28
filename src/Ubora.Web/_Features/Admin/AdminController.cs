using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Projects._Queries;
using Ubora.Domain.Users.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Domain.Users.SortSpecifications;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Users;
using Ubora.Web._Features._Shared.Paging;
using Ubora.Domain.Users.Specifications;
using Ubora.Domain.Users.Queries;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Web._Features.Admin
{
    [Authorize(Roles = ApplicationRole.Admin)]
    public partial class AdminController : UboraController
    {
        private readonly IApplicationUserManager _userManager;
        private readonly ManageUsersViewModel.Factory _manageUserViewModelFactory;

        public AdminController(IApplicationUserManager userManager, ManageUsersViewModel.Factory manageUserViewModelFactory)
        {
            _userManager = userManager;
            _manageUserViewModelFactory = manageUserViewModelFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData[nameof(PageTitle)] = "Administer UBORA";
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public IActionResult Diagnostics()
        {
            return View(nameof(Diagnostics));
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public virtual async Task<IActionResult> ManageUsers(int page = 1, string searchName = null)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                page = 1;
            }

            var userProfiles = QueryProcessor.ExecuteQuery(new SearchUserProfilesQuery
            {
                SearchFullName = searchName,
                Paging = new Paging(page, 15)
            });

            var viewModel = await _manageUserViewModelFactory.Create(userProfiles.ToList(), searchName, Pager.From(userProfiles));
           return View(nameof(ManageUsers), viewModel);
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public IActionResult ProjectsUnderReview([FromServices] ProjectUnderReviewViewModel.Factory projectFactory)
        {
            var projectsUnderReview = QueryProcessor.ExecuteQuery(new GetProjectsUnderReviewQuery())
                .OrderBy(x => x.Title);

            var projectsViewModel = new ProjectsUnderReviewViewModel
            {
                ProjectsUnderReview = projectsUnderReview.Select(projectFactory.Create)
            };

            return View(nameof(ProjectsUnderReview), projectsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> AddAdministratorRole(Guid userId, int page)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddToRoleAsync(user, ApplicationRole.Admin);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return Diagnostics();
            }

            return RedirectToAction(nameof(ManageUsers), new { page });
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> RemoveAdministratorRole(Guid userId, int page)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.RemoveFromRoleAsync(user, ApplicationRole.Admin);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await ManageUsers();
            }

            return RedirectToAction(nameof(ManageUsers), new { page });
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> AddMentorRole(Guid userId, int page)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddToRoleAsync(user, ApplicationRole.Mentor);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await ManageUsers();
            }

            return RedirectToAction(nameof(ManageUsers), new { page });
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> RemoveMentorRole(Guid userId, int page)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.RemoveFromRoleAsync(user, ApplicationRole.Mentor);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await ManageUsers();
            }

            return RedirectToAction(nameof(ManageUsers), new { page });
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> AddManagementGroupRole(Guid userId, int page)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddToRoleAsync(user, ApplicationRole.ManagementGroup);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await ManageUsers();
            }

            return RedirectToAction(nameof(ManageUsers), new { page });
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> RemoveManagementGroupRole(Guid userId, int page)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.RemoveFromRoleAsync(user, ApplicationRole.ManagementGroup);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return await ManageUsers();
            }

            return RedirectToAction(nameof(ManageUsers), new { page });
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
        public async Task<IActionResult> DeleteUser(DeleteUserViewModel model, int page)
        {
            if (!ModelState.IsValid)
            {
                return await ManageUsers();
            }

            var user = await _userManager.FindByEmailAsync(model.UserEmail);
            if (user == null)
            {
                ModelState.AddModelError("", $"User not found by e-mail: {model.UserEmail}");
                return await ManageUsers();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                ExecuteUserCommand(new DeleteUserCommand
                {
                    UserId = user.Id
                }, Notice.Success(SuccessTexts.UserDeleted));
            }
            else
            {
                AddIdentityErrorsToModelState(result);
            }

            if (!ModelState.IsValid)
            {
                return await ManageUsers();
            }

            Notices.NotifyOfSuccess($"User {user.Email} successfully deleted");
            return RedirectToAction(nameof(ManageUsers), new { page });
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
