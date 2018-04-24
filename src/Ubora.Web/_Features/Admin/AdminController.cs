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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Title"] = "Administer UBORA";
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public IActionResult Diagnostics()
        {
            return View(nameof(Diagnostics));
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> ManageUsers(int page = 1)
        {
            var userProfiles = QueryProcessor.Find(new MatchAll<UserProfile>(), new SortByFullNameAscendingSpecification(), 10, page);

            IReadOnlyDictionary<Guid, string> userFullNames = userProfiles.Select(userProfile => new
            {
                UserId = userProfile.UserId,
                FullName = userProfile.FullName
            })
            .ToDictionary(x => x.UserId, x => x.FullName);

            var userViewModels = new List<UserViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                if (userFullNames.ContainsKey(user.Id))
                {
                    userViewModels.Add(new UserViewModel
                    {
                        UserId = user.Id,
                        UserEmail = user.Email,
                        FullName = userFullNames[user.Id],
                        Roles = await _userManager.GetRolesAsync(user)
                    });
                }
            }

            var orderedViewModel = userViewModels.OrderBy(x => x.FullName).ToList();

            var manageUsersViewModel = new ManageUsersViewModel
            {
                Pager = Pager.From(userProfiles),
                Items = orderedViewModel
            };

            return View(nameof(ManageUsers), manageUsersViewModel);
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
        public async Task<IActionResult> AddAdministratorRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddToRoleAsync(user, ApplicationRole.Admin);

            if (!result.Succeeded)
            {
                AddIdentityErrorsToModelState(result);

                return Diagnostics();
            }

            return RedirectToAction(nameof(ManageUsers));
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

                return await ManageUsers();
            }

            return RedirectToAction(nameof(ManageUsers));
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

                return await ManageUsers();
            }

            return RedirectToAction(nameof(ManageUsers));
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

                return await ManageUsers();
            }

            return RedirectToAction(nameof(ManageUsers));
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
            return RedirectToAction(nameof(ManageUsers));
        }

        public class ManageUsersViewModel
        {
            public Pager Pager { get; set; }
            public List<UserViewModel> Items { get; set; }
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
