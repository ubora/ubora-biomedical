using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Projects.InviteMentors
{
    [Authorize(Roles = ApplicationRole.Admin)]
    public class InviteMentorsController : ProjectController
    {
        private readonly ApplicationUserManager _userManager;

        public InviteMentorsController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        [DisableProjectControllerAuthorization]
        [Authorize(Roles = ApplicationRole.Admin)]
        public IActionResult InviteMentors([FromServices]MentorsViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(this.ProjectId);

            return View(nameof(InviteMentors), model);
        }

        [HttpPost]
        [DisableProjectControllerAuthorization]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> InviteMentor(Guid userId, [FromServices]MentorsViewModel.Factory modelFactory)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException(userId.ToString());
            }

            if (!await _userManager.IsInRoleAsync(user, ApplicationRole.Mentor))
            {
                ModelState.AddModelError("", "User is not Ubora mentor.");
            }

            if (!ModelState.IsValid)
            {
                return InviteMentors(modelFactory);
            }

            ExecuteUserProjectCommand(new InviteProjectMentorCommand
            {
                UserId = userId
            });

            if (!ModelState.IsValid)
            {
                return InviteMentors(modelFactory);
            }

            Notices.Success("Mentor successfully invited.");

            return RedirectToAction(nameof(InviteMentors));
        }
    }
}
