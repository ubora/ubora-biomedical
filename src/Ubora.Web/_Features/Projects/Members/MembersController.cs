using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Authorization;
using Ubora.Domain.Projects;
using Ubora.Web.Services;
using Microsoft.AspNetCore.Identity;
using Ubora.Web.Data;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Authorization.Requirements;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.Members
{
    [ProjectRoute("[controller]")]
    public class MembersController : ProjectController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ImageStorageProvider _imageStorageProvider;

        public MembersController(SignInManager<ApplicationUser> signInManager, ImageStorageProvider imageStorageProvider)
        {
            _signInManager = signInManager;
            _imageStorageProvider = imageStorageProvider;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Members()
        {
            var isProjectMember = (await AuthorizationService.AuthorizeAsync(User, null, new IsProjectMemberRequirement())).Succeeded;
            var isAuthenticated = (await AuthorizationService.AuthorizeAsync(User, Policies.IsAuthenticatedUser)).Succeeded;

            var isAdmin = User.IsInRole(ApplicationRole.Admin);
            var canRemoveProjectMember = (await AuthorizationService.AuthorizeAsync(User, Policies.CanRemoveProjectMember)).Succeeded;
            var memberListItemViewModels = new List<ProjectMemberListViewModel.Item>();
            foreach (var userMembers in Project.Members.GroupBy(m => m.UserId))
            {
                var memberUserId = userMembers.Key;
                var userProfile = QueryProcessor.FindById<UserProfile>(memberUserId);
                var itemModel = new ProjectMemberListViewModel.Item
                {
                    UserId = memberUserId,
                    IsProjectLeader = userMembers.Any(x => x.IsLeader),
                    IsProjectMentor = userMembers.Any(x => x.IsMentor),
                    IsCurrentUser = (isAuthenticated && this.UserId == memberUserId),
                    FullName = userProfile.FullName,
                    ProfilePictureUrl = _imageStorageProvider.GetDefaultOrBlobUrl(userProfile),
                    CanRemoveProjectMember = (isAdmin && userMembers.Any(x => x.IsMentor)) || canRemoveProjectMember
                };
                memberListItemViewModels.Add(itemModel);
            }

            var isProjectLeader = isAuthenticated && Project.Members.Any(x => x.UserId == UserId && x.IsLeader);

            var model = new ProjectMemberListViewModel
            {
                Id = ProjectId,
                Members = memberListItemViewModels,
                IsProjectMember = isAuthenticated && isProjectMember,
                IsProjectLeader = isProjectLeader
            };

            return View(nameof(Members), model);
        }

        [Route(nameof(Invite))]
        public IActionResult Invite()
        {
            return View();
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
            }, Notice.Success(SuccessTexts.ProjectMemberInvited));

            if (!ModelState.IsValid)
            {
                return Invite();
            }

            return RedirectToAction(nameof(Members));
        }

        [Route(nameof(Join))]
        [DisableProjectControllerAuthorization]
        [Authorize(Policies.CanJoinProject)]
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
        [Route(nameof(Join))]
        [DisableProjectControllerAuthorization]
        [Authorize(Policies.CanJoinProject)]
        public IActionResult Join(JoinProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ExecuteUserProjectCommand(new JoinProjectCommand(), Notice.Success(SuccessTexts.RequestToJoinProjectSent));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Dashboard", "Dashboard", new { });
        }

        [Route(nameof(RemoveMemberByProjectLeader))]
        [Authorize(Policies.CanRemoveProjectMember)]
        public IActionResult RemoveMemberByProjectLeader(Guid memberId)
        {
            var removeMemberViewModel = new RemoveMemberViewModel
            {
                MemberId = memberId,
                MemberName = QueryProcessor.FindById<UserProfile>(memberId).FullName
            };

            return View(removeMemberViewModel);
        }

        [HttpPost]
        [Route(nameof(RemoveMemberByProjectLeader))]
        [Authorize(Policies.CanRemoveProjectMember)]
        public IActionResult RemoveMemberByProjectLeader(RemoveMemberViewModel removeMemberViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(removeMemberViewModel);
            }

            ExecuteUserProjectCommand(new RemoveMemberFromProjectCommand
            {
                UserId = removeMemberViewModel.MemberId
            }, Notice.Success(SuccessTexts.ProjectMemberRemoved));

            if (!ModelState.IsValid)
            {
                return View(removeMemberViewModel);
            }

            return RedirectToAction(nameof(Members));
        }

        [Route(nameof(RemoveMentorByAdmin))]
        [AllowAnonymous]
        [Authorize(Roles = ApplicationRole.Admin)]
        public IActionResult RemoveMentorByAdmin(Guid memberId)
        {
            var removeMemberViewModel = new RemoveMemberViewModel
            {
                MemberId = memberId,
                MemberName = QueryProcessor.FindById<UserProfile>(memberId).FullName
            };

            return View(removeMemberViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [Authorize(Roles = ApplicationRole.Admin)]
        [Route(nameof(RemoveMentorByAdmin))]
        public IActionResult RemoveMentorByAdmin(RemoveMemberViewModel removeMemberViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(removeMemberViewModel);
            }

            ExecuteUserProjectCommand(new RemoveMemberFromProjectCommand
            {
                UserId = removeMemberViewModel.MemberId
            }, Notice.Success(SuccessTexts.ProjectMemberRemoved));

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
            }, Notice.Success(SuccessTexts.LeftFromProject));

            if (!ModelState.IsValid)
            {
                return Leave();
            }

            return RedirectToAction("Dashboard", "Dashboard", new { });
        }
    }
}