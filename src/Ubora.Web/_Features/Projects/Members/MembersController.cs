using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users;
using Ubora.Domain.Notifications;
using System;
using Ubora.Domain.Projects.Members;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Authorization;
using System.Threading.Tasks;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Projects.Members
{
    public class MembersController : ProjectController
    {
        private readonly IAuthorizationService _authorizationService;

        public MembersController(ICommandQueryProcessor processor, IAuthorizationService authorizationService) : base(processor)
        {
            _authorizationService = authorizationService;
        }

        [AllowAnonymous]
        [Route(nameof(Members))]
        public async Task<IActionResult> Members()
        {
            var canRemoveProjectMembers = await _authorizationService.AuthorizeAsync(User, Policies.CanRemoveProjectMember);
            var isProjectMember = await _authorizationService.AuthorizeAsync(User, null, new IsProjectMemberRequirement());

            var model = new ProjectMemberListViewModel
            {
                Id = ProjectId,
                CanRemoveProjectMembers = canRemoveProjectMembers,
                Members = Project.Members.Select(m => new ProjectMemberListViewModel.Item
                {
                    UserId = m.UserId,
                    // TODO(Kaspar Kallas): Eliminate SELECT(N + 1)
                    FullName = FindById<UserProfile>(m.UserId).FullName,
                    IsProjectLeader = m.IsLeader
                }),
                IsProjectMember = isProjectMember
            };

            return View(model);
        }

        public IActionResult Invite()
        {
            var model = new InviteProjectMemberViewModel { ProjectId = ProjectId };

            return View(model);
        }

        [HttpPost]
        public IActionResult Invite(InviteProjectMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Invite();
            }

            ExecuteUserProjectCommand(new InviteMemberToProjectCommand
            {
                InvitedMemberEmail = model.Email
            });

            if (!ModelState.IsValid)
            {
                return Invite();
            }

            return RedirectToAction(nameof(Members), new { id = model.ProjectId });
        }

        [Authorize(Policy = nameof(Policies.CanRemoveProjectMember))]
        public IActionResult RemoveMember(Guid memberId)
        {
            var removeMemberViewModel = new RemoveMemberViewModel
            {
                MemberId = memberId,
                MemberName = FindById<UserProfile>(memberId).FullName
            };

            return View(removeMemberViewModel);
        }

        [HttpPost]
        [Authorize(Policy = nameof(Policies.CanRemoveProjectMember))]
        public IActionResult RemoveMember(RemoveMemberViewModel removeMemberViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(removeMemberViewModel);
            }

            ExecuteUserProjectCommand(new RemoveMemberFromProjectCommand
            {
                UserId = removeMemberViewModel.MemberId
            });

            if (!ModelState.IsValid)
            {
                return View(removeMemberViewModel);
            }

            return RedirectToAction(nameof(Members));
        }
    }
}
