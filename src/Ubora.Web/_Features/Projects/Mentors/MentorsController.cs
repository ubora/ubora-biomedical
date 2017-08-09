using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Projects.Members;
using Ubora.Web._Features.Users.UserList;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Projects.Mentors
{
    public class MentorsController : ProjectController
    {
        private readonly ApplicationUserManager _userManager;

        public MentorsController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        [DisableProjectControllerAuthorization]
        [Authorize(Roles = ApplicationRole.Admin)]
        public IActionResult InviteMentors()
        {
            var projectMentors = QueryProcessor.ExecuteQuery(new FindProjectMentorProfilesQuery
            {
                ProjectId = this.ProjectId
            });
            var uboraMentors = QueryProcessor.ExecuteQuery(new FindUboraMentorProfilesQuery());

            var model = new MentorsViewModel
            {
                UboraMentors = uboraMentors.Select(AutoMapper.Map<UserListItemViewModel>),
                ProjectMentors = projectMentors.Select(AutoMapper.Map<UserListItemViewModel>)
            };

            return View(nameof(InviteMentors), model);
        }

        [HttpPost]
        [DisableProjectControllerAuthorization]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> InviteMentor(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            if (!await _userManager.IsInRoleAsync(user, ApplicationRole.Mentor))
            {
                ModelState.AddModelError("", "User is not Ubora mentor.");
            }

            if (!ModelState.IsValid)
            {
                return InviteMentors();
            }

            // TODO(Kaspar Kallas): Invitation with notification
            ExecuteUserProjectCommand(new AssignProjectMentorCommand
            {
                UserId = userId
            });

            if (!ModelState.IsValid)
            {
                return InviteMentors();
            }

            return RedirectToAction(nameof(InviteMentors));
        }
    }
}
