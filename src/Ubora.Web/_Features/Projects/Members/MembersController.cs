using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Users;
using System;
using System.Threading.Tasks;
using Ubora.Domain.Projects.Members;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Authorization;
using Ubora.Domain.Projects;
using Ubora.Web.Services;
using Microsoft.AspNetCore.Identity;
using Ubora.Web.Data;
using Ubora.Domain.Notifications.Invitation;
using Ubora.Domain.Notifications.Join;

namespace Ubora.Web._Features.Projects.Members
{
    [ProjectRoute("[controller]")]
    public class MembersController : ProjectController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public MembersController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Members()
        {
            var canRemoveProjectMembers = await AuthorizationService.AuthorizeAsync(User, Policies.CanRemoveProjectMember);
            var isProjectMember = await AuthorizationService.AuthorizeAsync(User, null, new IsProjectMemberRequirement());
            var isAuthenticated = await AuthorizationService.AuthorizeAsync(User, Policies.IsAuthenticatedUser);

            var members = Project.Members.Select(m => new ProjectMemberListViewModel.Item
            {
                UserId = m.UserId,
                // TODO(Kaspar Kallas): Eliminate SELECT(N + 1)
                FullName = QueryProcessor.FindById<UserProfile>(m.UserId).FullName,
                IsProjectLeader = m.IsLeader,
                IsCurrentUser = isAuthenticated && UserId == m.UserId
            });

            var isProjectLeader = isAuthenticated && members.Any(x => x.UserId == UserId && x.IsProjectLeader);

            var model = new ProjectMemberListViewModel
            {
                Id = ProjectId,
                CanRemoveProjectMembers = canRemoveProjectMembers,
                Members = members,
                IsProjectMember = isAuthenticated && isProjectMember,
                IsProjectLeader = isProjectLeader
            };

            return View(nameof(Members), model);
        }

        [Route(nameof(Invite))]
        public IActionResult Invite()
        {
            var model = new InviteProjectMemberViewModel { ProjectId = ProjectId };

            return View(model);
        }

        [HttpPost]
        [Route(nameof(Invite))]
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

        [AllowAnonymous]
        [Route(nameof(Join))]
        public IActionResult Join(Guid projectId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                var returnUrl = Url.Action("Join", "Members", new { projectId = projectId });

                return RedirectToAction("SignInSignUp", "Account", new { returnUrl = returnUrl });
            }

            var project = QueryProcessor.FindById<Project>(projectId);

            var model = new JoinProjectViewModel
            {
                UserId = User.GetId(),
                ProjectId = projectId,
                ProjectName = project.Title
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Join))]
        public IActionResult Join(JoinProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ExecuteUserProjectCommand(new JoinProjectCommand());

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Dashboard", "Dashboard", new { });
        }

        [Route(nameof(RemoveMember))]
        [Authorize(Policy = nameof(Policies.CanRemoveProjectMember))]
        public IActionResult RemoveMember(Guid memberId)
        {
            var removeMemberViewModel = new RemoveMemberViewModel
            {
                MemberId = memberId,
                MemberName = QueryProcessor.FindById<UserProfile>(memberId).FullName
            };

            return View(removeMemberViewModel);
        }

        [HttpPost]
        [Route(nameof(RemoveMember))]
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

        [Route(nameof(Leave))]
        public IActionResult Leave()
        {
            return View(nameof(Leave));
        }

        [HttpPost]
        [Route(nameof(Leave))]
        public IActionResult LeaveProject()
        {
            if (!ModelState.IsValid)
            {
                return Leave();
            }

            ExecuteUserProjectCommand(new RemoveMemberFromProjectCommand
            {
                UserId = UserId
            });

            if (!ModelState.IsValid)
            {
                return Leave();
            }

            return RedirectToAction("Dashboard", "Dashboard", new { });
        }

        [HttpPost]
        [Route(nameof(AssignMeAsMentor))]
        [DisableProjectControllerAuthorization]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> AssignMeAsMentor()
        {
            ExecuteUserProjectCommand(new AssignProjectMentorCommand
            {
                UserId = this.UserId
            });

            if (!ModelState.IsValid)
            {
                return await Members();
            }

            return RedirectToAction(nameof(Members));
        }
    }
}
