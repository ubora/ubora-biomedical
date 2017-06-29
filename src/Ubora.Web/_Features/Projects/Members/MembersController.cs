using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
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
using Microsoft.AspNetCore.Mvc.Routing;
using Ubora.Domain.Notifications.Invitation;
using Ubora.Domain.Notifications.Join;

namespace Ubora.Web._Features.Projects.Members
{
    public class MembersController : ProjectController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IAuthorizationService _authorizationService;

        public MembersController(
            ICommandQueryProcessor processor,
            SignInManager<ApplicationUser> signInManager,
            IUrlHelperFactory urlHelperFactory,
            IAuthorizationService authorizationService) : base(processor)
        {
            _signInManager = signInManager;
            _urlHelperFactory = urlHelperFactory;
            _authorizationService = authorizationService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Members()
        {
            var canRemoveProjectMembers = await _authorizationService.AuthorizeAsync(User, Policies.CanRemoveProjectMember);
            var isProjectMember = await _authorizationService.AuthorizeAsync(User, null, new IsProjectMemberRequirement());
            var isAuthenticated = await _authorizationService.AuthorizeAsync(User, Policies.IsAuthenticatedUser);

            var members = Project.Members.Select(m => new ProjectMemberListViewModel.Item
            {
                UserId = m.UserId,
                // TODO(Kaspar Kallas): Eliminate SELECT(N + 1)
                FullName = FindById<UserProfile>(m.UserId).FullName,
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

        [AllowAnonymous]
        public IActionResult Join(Guid projectId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                var urlHelper = _urlHelperFactory.GetUrlHelper(ControllerContext);
                var returnUrl = urlHelper.Action("Join", "Members", new { projectId = projectId });

                return RedirectToAction("SignInSignUp", "Account", new { returnUrl = returnUrl });
            }

            var project = FindById<Project>(projectId);

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

        public IActionResult Leave()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LeaveProject()
        {
            if (!ModelState.IsValid)
            {
                return View("Leave");
            }

            ExecuteUserProjectCommand(new RemoveMemberFromProjectCommand
            {
                UserId = UserId
            });

            if (!ModelState.IsValid)
            {
                return View("Leave");
            }

            return RedirectToAction("Dashboard", "Dashboard", new { });
        }

        [DisableProjectControllerAuthorization]
        [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPost]
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
