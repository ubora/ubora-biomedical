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
using Ubora.Web.Services;
using Microsoft.AspNetCore.Identity;
using Ubora.Web.Data;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Ubora.Web._Features.Projects.Members
{
    public class MembersController : ProjectController
    {
        private SignInManager<ApplicationUser> _signInManager;
        private IUrlHelperFactory _urlHelperFactory;

        public MembersController(
            ICommandQueryProcessor processor,
            SignInManager<ApplicationUser> signInManager,
            IUrlHelperFactory urlHelperFactory) : base(processor)
        {
            _signInManager = signInManager;
            _urlHelperFactory = urlHelperFactory;
        }

        [Route(nameof(Members))]
        public async Task<IActionResult> Members()
        {
            var canRemoveProjectMembers = Project.DoesSatisfy(new HasLeader(UserInfo.UserId));

            var model = new ProjectMemberListViewModel
            {
                Id = ProjectId,
                CanRemoveProjectMembers = canRemoveProjectMembers,
                Members = Project.Members.Select(m => new ProjectMemberListViewModel.Item
                {
                    UserId = m.UserId,
                    // TODO(Kaspar Kallas): Eliminate SELECT(N + 1)
                    FullName = FindById<UserProfile>(m.UserId).FullName,
                    IsProjectLeader = m.IsLeader,
                    IsCurrentUser = UserInfo.UserId == m.UserId
                })
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

            ExecuteUserProjectCommand(new JoinProjectCommand { AskingToJoin = User.GetId() });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Home", new { });
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

            return RedirectToAction("Index", "Home", new { });
        }
    }
}
