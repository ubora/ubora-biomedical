﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Data;
using Ubora.Web.Services;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Web.Authorization;

namespace Ubora.Web._Features.Projects.InviteMentors
{
    public class InviteMentorsController : ProjectController
    {
        private readonly ApplicationUserManager _userManager;

        public InviteMentorsController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        [DisableProjectControllerAuthorization]
        [Authorize(Policies.CanInviteMentors)]
        public IActionResult InviteMentors([FromServices]MentorsViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(this.ProjectId);

            return View(nameof(InviteMentors), model);
        }

        [HttpPost]
        [DisableProjectControllerAuthorization]
        [Authorize(Policies.CanInviteMentors)]
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
            }, Notice.Success(SuccessTexts.ProjectMentorInvited));

            if (!ModelState.IsValid)
            {
                return InviteMentors(modelFactory);
            }

            return RedirectToAction(nameof(InviteMentors));
        }
    }
}
