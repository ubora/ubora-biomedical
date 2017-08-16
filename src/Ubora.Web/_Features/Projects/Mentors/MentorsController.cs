using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;
using Ubora.Web._Features._Shared.Notices;

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

            ShowNotice(new Notice("Mentor successfully invited.", NoticeType.Success));

            return RedirectToAction(nameof(InviteMentors));
        }
    }
}
